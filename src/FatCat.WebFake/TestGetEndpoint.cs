using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake;

public class TestGetEndpoint : Endpoint
{
    [HttpGet("test")]
    public WebResult TestGet()
    {
        ConsoleLog.WriteCyan("Test Get Endpoint");

        return WebResult.Ok($"ACK from Test Get Endpoint | {DateTime.Now:h:mm:ss tt}");
    }
}
