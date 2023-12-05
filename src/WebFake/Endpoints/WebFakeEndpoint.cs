using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace FatCat.WebFake.Endpoints;

public abstract class WebFakeEndpoint(
	IFatCatCache<ResponseCacheItem> cache,
	IWebFakeSettings settings,
	IThread thread
) : Endpoint
{
	protected readonly IFatCatCache<ResponseCacheItem> cache = cache;
	protected readonly IThread thread = thread;

	protected string ResponsePath
	{
		get => $"/{settings.FakeId}/response";
	}

	protected string GetPath()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		return displayUri.PathAndQuery.ToLower();
	}

	protected bool IsResponseEntry()
	{
		var path = GetPath();

		return path.StartsWith(ResponsePath);
	}

	protected async Task<WebResult> ProcessRequest()
	{
		var path = GetPath();

		var cacheItem = cache.Get(path);

		if (cacheItem?.Entry?.Response == null)
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
}
