﻿using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Models;

namespace FatCat.WebFake.Endpoints;

public class PutEndpoint(
	IFatCatCache<ResponseCacheItem> cache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> requestCache
) : WebFakeEndpoint(cache, settings, thread, requestCache)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Put;
	}

	public override async Task<WebResult> DoAction()
	{
		return await ProcessRequest();
	}
}
