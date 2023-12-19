using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFakeApi.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Endpoint = FatCat.Toolkit.WebServer.Endpoint;

namespace FatCat.WebFake.Endpoints;

public abstract class WebFakeEndpoint(
	IFatCatCache<ResponseCacheItem> responseCache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> clientRequestCache,
	IGenerator generator,
	IDateTimeUtilities dateTimeUtilities
) : Endpoint
{
	protected readonly IFatCatCache<ClientRequestCacheItem> clientRequestCache = clientRequestCache;
	protected readonly IFatCatCache<ResponseCacheItem> responseCache = responseCache;
	protected IWebFakeSettings settings = settings;

	protected string ResponsePath
	{
		get => $"/{settings.FakeId}/response".ToLower();
	}

	protected abstract HttpVerb SupportedVerb { get; }

	public abstract Task<WebResult> DoAction();

	protected string GetPath()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		return displayUri.PathAndQuery.ToLower();
	}

	protected async Task<string> GetRequestBody()
	{
		using var reader = new StreamReader(Request.Body);

		return await reader.ReadToEndAsync();
	}

	protected bool IsResponseEntry()
	{
		var path = GetPath();

		return path.StartsWith(ResponsePath);
	}

	protected async Task<WebResult> ProcessRequest()
	{
		await SaveClientRequest();

		var path = GetPath();

		var cacheId = $"{SupportedVerb}-{path}";

		var cacheItem = responseCache.Get(cacheId);

		if (cacheItem is null)
		{
			return WebResult.NotFound();
		}

		if (cacheItem.Entry?.Response == null || cacheItem.Entry.Verb != SupportedVerb)
		{
			return WebResult.NotFound();
		}

		var response = cacheItem.Entry.Response;

		if (response.Delay is not null)
		{
			await thread.Sleep(response.Delay.Value);
		}

		foreach (var header in response.Headers)
		{
			Response.Headers.TryAdd(header.Key, new StringValues(header.Value));
		}

		var webResult = new WebResult(response.HttpStatusCode, response.Body)
		{
			ContentType = response.ContentType
		};

		return webResult;
	}

	private Dictionary<string, string> GetRequestHeaders()
	{
		var headers = new Dictionary<string, string>();

		foreach (var currentHeader in Request.Headers)
		{
			var value = currentHeader.Value[0];

			headers.Add(currentHeader.Key, value);
		}

		return headers;
	}

	private async Task SaveClientRequest()
	{
		var clientRequest = new ClientRequest
		{
			ContentType = Request.ContentType,
			Headers = GetRequestHeaders(),
			Verb = SupportedVerb,
			RequestBody = await GetRequestBody(),
			RequestId = generator.NewId(),
			RequestTime = dateTimeUtilities.UtcNow()
		};

		clientRequestCache.Add(new ClientRequestCacheItem(clientRequest));
	}
}
