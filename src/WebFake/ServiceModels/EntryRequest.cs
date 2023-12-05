using FatCat.Toolkit;

namespace FatCat.WebFake.ServiceModels;

public class EntryRequest : EqualObject
{
	public HttpVerb Verb { get; set; }

	public string Path { get; set; }

	public EntryResponse Response { get; set; }
}
