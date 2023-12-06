using FatCat.Toolkit;
using FatCat.Toolkit.Caching;

namespace FatCat.WebFake.Models;

public class ResponseCacheItem : EqualObject, ICacheItem
{
	public string CacheId
	{
		get => $"{Entry.Verb}-{Entry.Path.ToLower()}";
	}

	public EntryRequest Entry { get; set; }
}
