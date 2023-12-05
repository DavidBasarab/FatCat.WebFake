using System.Net;
using FatCat.Toolkit;

namespace FatCat.WebFake.ServiceModels;

public class EntryResponse : EqualObject
{
	public string Body { get; set; }

	public string ContentType { get; set; }

	public TimeSpan? Delay { get; set; }

	public Dictionary<string, string> Headers { get; set; } = new();

	public HttpStatusCode HttpStatusCode { get; set; }
}
