using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace FatCat.WebFake.Endpoints;

public class GetEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings, IThread thread)
	: WebFakeEndpoint(cache, settings, thread)
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
