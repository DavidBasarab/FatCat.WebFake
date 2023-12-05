using FatCat.Toolkit.Caching;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class GetEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings)
	: WebFakeEndpoint(cache, settings)
{
	[HttpGet("{*url}")]
	public async Task<WebResult> DoGet()
	{
		await Task.CompletedTask;

		if (IsResponseEntry())
		{
			var allItems = cache.GetAll();

			return Ok(allItems.Select(i => i.Entry));
		}

		var path = GetPath();

		var cacheItem = cache.Get(path);

		return WebResult.NotFound();
	}
}
