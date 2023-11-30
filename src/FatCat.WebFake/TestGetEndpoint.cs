using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FatCat.WebFake;

public class TestGetEndpoint : Endpoint
{
    [HttpGet("{*url}")]
    public WebResult TestGet()
    {
        var displayUrl = Request.GetDisplayUrl();

        ConsoleLog.WriteCyan($"Test Get Endpoint from | <{displayUrl}>");

        return WebResult.Ok($"ACK from Test Get Endpoint | {DateTime.Now:h:mm:ss tt}");
    }
}
