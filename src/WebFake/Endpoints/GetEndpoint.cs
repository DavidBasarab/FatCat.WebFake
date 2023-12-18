using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFakeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class GetEndpoint(
	IFatCatCache<ResponseCacheItem> responseCache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> requestCache,
	IGenerator generator,
	IDateTimeUtilities dateTimeUtilities
) : WebFakeEndpoint(responseCache, settings, thread, requestCache, generator, dateTimeUtilities)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Get;
	}

	private string ClientRequestPath
	{
		get => $"/{settings.FakeId}/request";
	}

	[HttpGet("{*url}")]
	public override async Task<WebResult> DoAction()
	{
		if (IsResponseEntry())
		{
			var allItems = responseCache.GetAll();

			return Ok(allItems.Select(i => i.Entry));
		}

		if (IsGetClientRequest())
		{
			var requestItems = clientRequestCache.GetAll();

			return Ok(requestItems.Select(i => i.Request));
		}

		return await ProcessRequest();
	}

	private bool IsGetClientRequest()
	{
		var path = GetPath();

		return path.StartsWith(ClientRequestPath);
	}
}
