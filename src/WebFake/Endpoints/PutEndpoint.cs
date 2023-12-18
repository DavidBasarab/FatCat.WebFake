using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.WebServer;
using FatCat.WebFakeApi.Models;

namespace FatCat.WebFake.Endpoints;

public class PutEndpoint(
	IFatCatCache<ResponseCacheItem> cache,
	IWebFakeSettings settings,
	IThread thread,
	IFatCatCache<ClientRequestCacheItem> requestCache,
	IGenerator generator,
	IDateTimeUtilities dateTimeUtilities
) : WebFakeEndpoint(cache, settings, thread, requestCache, generator, dateTimeUtilities)
{
	protected override HttpVerb SupportedVerb
	{
		get => HttpVerb.Put;
	}

	public override Task<WebResult> DoAction()
	{
		return ProcessRequest();
	}
}
