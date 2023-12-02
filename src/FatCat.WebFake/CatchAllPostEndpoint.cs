using FatCat.Toolkit.Caching;
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
		var displayUri = new Uri(Request.GetDisplayUrl());

		if (!displayUri.PathAndQuery.StartsWith($"/{webFakeSettings.FakeId}"))
		{
			return NotImplemented();
		}

		using var reader = new StreamReader(Request.Body);

		var body = await reader.ReadToEndAsync();

		var entryRequest = JsonConvert.DeserializeObject<EntryRequest>(body);

		responseCache.Add(new ResponseCacheItem { Entry = entryRequest });

		return NotImplemented();
	}
}
