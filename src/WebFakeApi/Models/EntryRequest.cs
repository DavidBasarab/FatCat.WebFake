using FatCat.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FatCat.WebFakeApi.Models;

public class EntryRequest : EqualObject
{
	public string Path { get; set; }

	public EntryResponse Response { get; set; }

	[JsonConverter(typeof(StringEnumConverter))]
	public HttpVerb Verb { get; set; }
}
