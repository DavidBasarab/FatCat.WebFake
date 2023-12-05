using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFake.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class DeleteEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings, IThread thread)
	: WebFakeEndpoint(cache, settings, thread)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Delete;
	}

	[HttpDelete("{*url}")]
	public WebResult ProcessDelete()
	{
		if (IsResponseEntry())
		{
			return RemoveResponseEntry();
		}

		return NotImplemented();
	}

	private WebResult RemoveResponseEntry()
	{
		var fullPath = GetPath();

		var pathToRemove = fullPath.Replace($"{ResponsePath}", string.Empty);

		cache.Remove(pathToRemove.ToLower());

		return Ok(ResponseCodes.EntryRemoved);
	}
}
