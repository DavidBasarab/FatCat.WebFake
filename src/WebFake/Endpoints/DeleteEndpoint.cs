using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Models;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class DeleteEndpoint(
	IFatCatCache<ResponseCacheItem> responseCache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> requestCache
) : WebFakeEndpoint(responseCache, settings, thread, requestCache)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Delete;
	}

	[HttpDelete("{*url}")]
	public override async Task<WebResult> DoAction()
	{
		if (IsResponseEntry())
		{
			return RemoveResponseEntry();
		}

		return await ProcessRequest();
	}

	private WebResult RemoveResponseEntry()
	{
		var fullPath = GetPath();

		var pathToRemove = fullPath.Replace($"{ResponsePath}", string.Empty);

		responseCache.Remove(pathToRemove.ToLower());

		return Ok(ResponseCodes.EntryRemoved);
	}
}
