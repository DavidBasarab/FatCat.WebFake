using FatCat.Toolkit;
using FatCat.Toolkit.Caching;

namespace FatCat.WebFake.Models;

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
