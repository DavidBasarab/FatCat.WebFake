using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;

namespace FatCat.WebFake.Endpoints;

public abstract class WebFakeEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings) : Endpoint
{
	protected readonly IFatCatCache<ResponseCacheItem> cache = cache;
	protected readonly IWebFakeSettings settings = settings;

	public string ResponsePath
	{
		get => $"/{settings.FakeId}/response";
	}

	protected string GetPath()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		ConsoleLog.WriteMagenta($"DisplayUri: {displayUri}");

		return displayUri.PathAndQuery;
	}

	protected bool IsResponseEntry()
	{
		var path = GetPath();

		return path.StartsWith(ResponsePath);
	}
}
