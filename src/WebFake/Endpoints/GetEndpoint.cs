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

		if (cacheItem?.Entry?.Response != null)
		{
			var response = cacheItem.Entry.Response;

			return new WebResult(response.HttpStatusCode, response.Body);
		}

		return WebResult.NotFound();
	}
}
