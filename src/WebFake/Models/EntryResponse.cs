using System.Net;
using FatCat.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FatCat.WebFake.Models;

public class EntryResponse : EqualObject
{
	public string Body { get; set; }

	public string ContentType { get; set; }

	public TimeSpan? Delay { get; set; }

	public Dictionary<string, string> Headers { get; set; } = new();

	[JsonConverter(typeof(StringEnumConverter))]
	public HttpStatusCode HttpStatusCode { get; set; }
}
