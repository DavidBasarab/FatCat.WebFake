using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake;

public class CatchAllGetEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings)
	: CatchAllEndpoint(cache, settings)
{
	[HttpGet("{*url}")]
	public WebResult ProcessGet()
	{
		var displayUrl = Request.GetDisplayUrl();

		ConsoleLog.WriteCyan($"Test Get Endpoint from | <{displayUrl}>");
		ConsoleLog.WriteMagenta($"Configuration testing | settings.UniqueId := <{settings.FakeId}>");

		return WebResult.Ok($"ACK from Test Get Endpoint | {DateTime.Now:h:mm:ss tt}");
	}
}
