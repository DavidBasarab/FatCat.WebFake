using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;

namespace FatCat.WebFake.Endpoints;

public abstract class CatchAllEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings) : Endpoint
{
	protected readonly IFatCatCache<ResponseCacheItem> cache = cache;
	protected readonly IWebFakeSettings settings = settings;

	protected bool IsResponseEntry()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		ConsoleLog.WriteMagenta($"DisplayUri: {displayUri}");

		return displayUri.PathAndQuery.StartsWith($"/{settings.FakeId}/response");
	}
}
