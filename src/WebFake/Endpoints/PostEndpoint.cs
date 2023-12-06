using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FatCat.WebFake.Endpoints;

public class PostEndpoint(
	IFatCatCache<ResponseCacheItem> responceCache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> requestCache
) : WebFakeEndpoint(responceCache, settings, thread, requestCache)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Post;
	}

	[HttpPost("{*url}")]
	public override async Task<WebResult> DoAction()
	{
		if (IsResponseEntry())
		{
			return await AddResponseEntry();
		}

		return await ProcessRequest();
	}

	private async Task<WebResult> AddResponseEntry()
	{
		using var reader = new StreamReader(Request.Body);

		var body = await reader.ReadToEndAsync();

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

		responseCache.Add(new ResponseCacheItem { Entry = entryRequest });

		return Ok(ResponseCodes.EntryAdded);
	}

	private bool IsInCache(EntryRequest entryRequest)
	{
		var cacheId = $"{entryRequest.Verb}-{entryRequest.Path}";

		return responseCache.InCache(cacheId);
	}
}
