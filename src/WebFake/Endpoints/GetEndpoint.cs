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

		if (cacheItem?.Entry?.Response == null)
		{
			return WebResult.NotFound();
		}

		var response = cacheItem.Entry.Response;

		var webResult = new WebResult(response.HttpStatusCode, response.Body)
		{
			ContentType = response.ContentType
		};

		return webResult;
	}
}
