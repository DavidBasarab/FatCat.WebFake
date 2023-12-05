using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;

namespace FatCat.WebFake.Endpoints;

public abstract class WebFakeEndpoint(
	IFatCatCache<ResponseCacheItem> cache,
	IWebFakeSettings settings,
	IThread thread
) : Endpoint
{
	protected readonly IFatCatCache<ResponseCacheItem> cache = cache;
	protected readonly IThread thread = thread;

	protected string ResponsePath
	{
		get => $"/{settings.FakeId}/response";
	}

	protected string GetPath()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		ConsoleLog.WriteMagenta($"DisplayUri: {displayUri}");

		return displayUri.PathAndQuery.ToLower();
	}

	protected bool IsResponseEntry()
	{
		var path = GetPath();

		return path.StartsWith(ResponsePath);
	}
}
