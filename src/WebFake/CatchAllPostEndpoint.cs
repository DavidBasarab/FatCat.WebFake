using FatCat.Toolkit.Caching;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FatCat.WebFake;

public class CatchAllPostEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings)
	: CatchAllEndpoint(cache, settings)
{
	[HttpPost("{*url}")]
	public async Task<WebResult> ProcessCatchAll()
	{
		if (!IsResponseEntry())
		{
			return NotImplemented();
		}

		return await AddResponseEntry();
	}

	private async Task<WebResult> AddResponseEntry()
	{
		using var reader = new StreamReader(Request.Body);

		var body = await reader.ReadToEndAsync();

		var entryRequest = JsonConvert.DeserializeObject<EntryRequest>(body);

		entryRequest.Path = entryRequest.Path.ToLower();

		if (cache.InCache(entryRequest.Path))
		{
			return BadRequest(ResponseCodes.EntryAlreadyExists);
		}

		cache.Add(new ResponseCacheItem { Entry = entryRequest });

		return Ok(ResponseCodes.EntryAdded);
	}
}
