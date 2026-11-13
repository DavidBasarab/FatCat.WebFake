using System.Net;
using System.Text.Json.Serialization;
using FatCat.Toolkit;

namespace FatCat.WebFakeApi.Models;

public class EntryResponse : EqualObject
{
	public string Body { get; set; }

	public string ContentType { get; set; }

	public TimeSpan? Delay { get; set; }

	public Dictionary<string, string> Headers { get; set; } = new();

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public HttpStatusCode HttpStatusCode { get; set; }
}
