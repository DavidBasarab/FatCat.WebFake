using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.WebFakeApi.Models;

namespace FatCat.WebFake;

public class ClientRequestCacheItem : EqualObject, ICacheItem
{
	public string CacheId
	{
		get => Request.RequestId;
	}

	public ClientRequest Request { get; set; }

	public ClientRequestCacheItem(ClientRequest request)
	{
		Request = request;
	}

	public ClientRequestCacheItem() { }
}
