using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

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

		return await ProcessRequest();
	}
}
