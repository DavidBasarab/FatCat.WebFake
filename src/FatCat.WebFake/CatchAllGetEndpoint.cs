﻿using FatCat.Toolkit.Console;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FatCat.WebFake;

public class CatchAllGetEndpoint(IWebFakeSettings settings) : Endpoint
{
    [HttpGet("{*url}")]
    public WebResult TestGet()
    {
        var displayUrl = Request.GetDisplayUrl();

        ConsoleLog.WriteCyan($"Test Get Endpoint from | <{displayUrl}>");
        ConsoleLog.WriteMagenta($"Configuration testing | settings.UniqueId := <{settings.UniqueId}>");

        return WebResult.Ok($"ACK from Test Get Endpoint | {DateTime.Now:h:mm:ss tt}");
    }
}