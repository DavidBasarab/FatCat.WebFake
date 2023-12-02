using FatCat.Toolkit;

namespace FatCat.WebFake.ServiceModels;

public class EntryRequest : EqualObject
{
	public string HttpMethod { get; set; }

	public string Path { get; set; }

	public EntryResponse Response { get; set; }
}
