using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFakeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FatCat.WebFake.Endpoints;

public class PostEndpoint(
	IFatCatCache<ResponseCacheItem> responseCache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> requestCache,
	IGenerator generator,
	IDateTimeUtilities dateTimeUtilities
) : WebFakeEndpoint(responseCache, settings, thread, requestCache, generator, dateTimeUtilities)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Post;
	}

	[HttpPost("{*url}")]
	public override Task<WebResult> DoAction()
	{
		return IsResponseEntry() ? AddResponseEntry() : ProcessRequest();
	}

	private async Task<WebResult> AddResponseEntry()
	{
		var body = await GetRequestBody();

		var entryRequest = JsonConvert.DeserializeObject<EntryRequest>(body);

		entryRequest.Path = entryRequest.Path.ToLower();

		if (!entryRequest.Path.StartsWith("/"))
		{
			return BadRequest(ResponseCodes.PathMustStartWithSlash);
		}

		if (IsInCache(entryRequest))
		{
			return BadRequest(ResponseCodes.EntryAlreadyExists);
		}

		var cacheItem = new ResponseCacheItem { Entry = entryRequest };

		responseCache.Add(cacheItem);

		ConsoleLog.Write($"Added Entry <{cacheItem.CacheId}>");

		return Ok(ResponseCodes.EntryAdded);
	}

	private bool IsInCache(EntryRequest entryRequest)
	{
		var cacheId = $"{entryRequest.Verb}-{entryRequest.Path}";

		return responseCache.InCache(cacheId);
	}
}
