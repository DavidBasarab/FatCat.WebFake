﻿using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace FatCat.WebFake.Endpoints;

public abstract class WebFakeEndpoint(
	IFatCatCache<ResponseCacheItem> responseCache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> clientRequestCache
) : Endpoint
{
	protected readonly IFatCatCache<ResponseCacheItem> responseCache = responseCache;
	protected readonly IFatCatCache<ClientRequestCacheItem> clientRequestCache = clientRequestCache;

	protected string ResponsePath
	{
		get => $"/{settings.FakeId}/response";
	}

	protected abstract HttpVerb SupportedVerb { get; }

	public abstract Task<WebResult> DoAction();

	protected string GetPath()
	{
		var displayUri = new Uri(Request.GetDisplayUrl());

		return displayUri.PathAndQuery.ToLower();
	}

	protected bool IsResponseEntry()
	{
		var path = GetPath();

		return path.StartsWith(ResponsePath);
	}

	protected async Task<WebResult> ProcessRequest()
	{
		var path = GetPath();

		var cacheId = $"{SupportedVerb}-{path}";

		var cacheItem = responseCache.Get(cacheId);

		if (cacheItem is null)
		{
			return WebResult.NotFound();
		}

		if (cacheItem.Entry?.Response == null || cacheItem.Entry.Verb != SupportedVerb)
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
