using System.Text.Json.Serialization;
using FatCat.Toolkit;

namespace FatCat.WebFakeApi.Models;

public class ClientRequest : EqualObject
{
	public string ContentType { get; set; }

	public Dictionary<string, string> Headers { get; set; }

	public string RequestBody { get; set; }

	public string RequestId { get; set; }

	public DateTime RequestTime { get; set; }

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public HttpVerb Verb { get; set; }
}
