using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Models;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class GetEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings, IThread thread)
	: WebFakeEndpoint(cache, settings, thread)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Get;
	}

	[HttpGet("{*url}")]
	public override async Task<WebResult> DoAction()
	{
		if (IsResponseEntry())
		{
			ConsoleLog.WriteMagenta("Getting all items");

			var allItems = cache.GetAll();

			ConsoleLog.WriteMagenta($"Returning all items => {allItems.Count}");

			return Ok(allItems.Select(i => i.Entry));
		}

		return await ProcessRequest();
	}
}
