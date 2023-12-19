using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFakeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class DeleteEndpoint(
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

		var pathToRemove = fullPath.Replace($"{ResponsePath}", string.Empty).Remove(0, 1);

		var cacheId = pathToRemove.ToLower();

		if (!responseCache.InCache(cacheId))
		{
			return BadRequest("path-not-found");
		}

		responseCache.Remove(cacheId);

		ConsoleLog.Write($"Removed Entry <{cacheId}>");

		return Ok(ResponseCodes.EntryRemoved);
	}
}
