// See https://aka.ms/new-console-template for more information

using System.Net;
using FatCat.WebFakeApi;
using FatCat.WebFakeApi.Models;

var api = new WebFakeAPi(new Uri("http://localhost:14888"), "david");

var entryRequest = new EntryRequest()
{
	Path = "asus/rog-strix-rtx3080-10g-gaming",
	Verb = HttpVerb.Get,
	Response = new EntryResponse() { Body = "HELLO WORLD", HttpStatusCode = HttpStatusCode.OK }
};

api.CreateEntryRequest(entryRequest);