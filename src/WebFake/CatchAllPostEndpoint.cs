using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.ServiceModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FatCat.WebFake;

public class CatchAllPostEndpoint(IFatCatCache<ResponseCacheItem> responseCache, IWebFakeSettings webFakeSettings)
	: Endpoint
{
	[HttpPost("{*url}")]
	public async Task<WebResult> ProcessCatchAll()
	{
		if (!IsSetResponseEntry())
		{
			return NotImplemented();
		}

		return await AddResponseEntry(responseCache);
	}

	private async Task<WebResult> AddResponseEntry(IFatCatCache<ResponseCacheItem> responseCache)
	{
		using var reader = new StreamReader(Request.Body);

		var body = await reader.ReadToEndAsync();

		var entryRequest = JsonConvert.DeserializeObject<EntryRequest>(body);

		entryRequest.Path = entryRequest.Path.ToLower();

		if (responseCache.InCache(entryRequest.Path))
		{
			return BadRequest("entry-already-exists");
		}

		responseCache.Add(new ResponseCacheItem { Entry = entryRequest });

		return Ok("entry-added");
	}

	private bool IsSetResponseEntry()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		ConsoleLog.WriteMagenta($"DisplayUri: {displayUri}");

		return displayUri.PathAndQuery.StartsWith($"/{webFakeSettings.FakeId}/response");
	}
}
