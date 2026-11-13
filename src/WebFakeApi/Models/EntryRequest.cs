using System.Text.Json.Serialization;
using FatCat.Toolkit;

namespace FatCat.WebFakeApi.Models;

public class EntryRequest : EqualObject
{
	public string Path { get; set; }

	public EntryResponse Response { get; set; }

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public HttpVerb Verb { get; set; }
}
