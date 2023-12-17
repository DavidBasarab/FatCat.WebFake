using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.WebFakeApi.Models;

namespace FatCat.WebFake;

public class ResponseCacheItem : EqualObject, ICacheItem
{
	public string CacheId
	{
		get => $"{Entry.Verb}-{Entry.Path}".ToLower();
	}

	public EntryRequest Entry { get; set; }
}
