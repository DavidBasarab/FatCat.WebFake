namespace FatCat.WebFake.Models;

public class ClientRequest
{
	public string ContentType { get; set; }

	public Dictionary<string, string> Headers { get; set; }

	public string RequestBody { get; set; }

	public string RequestId { get; set; }

	public DateTime RequestTime { get; set; }

	public HttpVerb Verb { get; set; }
}
