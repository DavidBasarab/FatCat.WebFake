﻿using FatCat.Toolkit.Caching;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake;

public class CatchAllGetEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings)
	: CatchAllEndpoint(cache, settings)
{
	[HttpGet("{*url}")]
	public WebResult ProcessGet()
	{
		if (IsResponseEntry())
		{
			var allItems = cache.GetAll();

			return Ok(allItems.Select(i => i.Entry));
		}

		return WebResult.Ok($"ACK from Test Get Endpoint | {DateTime.Now:h:mm:ss tt}");
	}
}
