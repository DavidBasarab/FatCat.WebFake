using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake;

public class TestEndpoint : Endpoint
{
	[HttpGet("api/test")]
	public WebResult TestGet()
	{
		ConsoleLog.WriteCyan("Got a test get request");

		return WebResult.Ok($"Got this {DateTime.Now:hh:mm:ss t z}");
	}
}
