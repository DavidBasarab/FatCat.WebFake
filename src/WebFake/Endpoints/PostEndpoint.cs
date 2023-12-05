using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FatCat.WebFake.Endpoints;

public class PostEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings, IThread thread)
	: WebFakeEndpoint(cache, settings, thread)
{
	[HttpPost("{*url}")]
	public async Task<WebResult> DoPost()
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

		if (cache.InCache(entryRequest.Path))
		{
			return BadRequest(ResponseCodes.EntryAlreadyExists);
		}

		cache.Add(new ResponseCacheItem { Entry = entryRequest });

		return Ok(ResponseCodes.EntryAdded);
	}
}
