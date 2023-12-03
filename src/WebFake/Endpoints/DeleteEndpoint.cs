using FatCat.Toolkit.Caching;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class DeleteEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings)
	: WebFakeEndpoint(cache, settings)
{
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

		var pathToRemove = fullPath.Replace($"{ResponsePath}/", string.Empty);

		cache.Remove(pathToRemove.ToLower());

		return Ok(ResponseCodes.EntryRemoved);
	}
}
