using FatCat.Toolkit.Caching;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake.Endpoints;

public class CatchAllDeleteEndpoint(IFatCatCache<ResponseCacheItem> cache, IWebFakeSettings settings)
	: CatchAllEndpoint(cache, settings)
{
	[HttpDelete("{*url}")]
	public WebResult ProcessDelete()
	{
		return NotImplemented();
	}
}
