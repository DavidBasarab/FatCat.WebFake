using System.Net;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi;
using FatCat.WebFakeApi.Models;

var fakeUri = new Uri("http://localhost:14888");

var api = new WebFakeAPi(fakeUri, "david");

var entryRequest = new EntryRequest
{
	Path = "asus/rog-strix-rtx3080-10g-gaming",
	Verb = HttpVerb.Get,
	Response = new EntryResponse { Body = "HELLO WORLD", HttpStatusCode = HttpStatusCode.OK }
};

await api.CreateEntryRequest(entryRequest);

var webCallerFactory = new WebCallerFactory(new ToolkitLogger(), new JsonOperations());

var webCaller = webCallerFactory.GetWebCaller(fakeUri);

var response = await webCaller.Get(entryRequest.Path);

if (response.IsUnsuccessful)
{
	ConsoleLog.WriteRed($"Response failed.  Status code: {response.StatusCode} | Content <{response.Content}>");
}
else
{
	ConsoleLog.WriteGreen(
		$"Response succeeded.  Status code: {response.StatusCode} | Content <{response.Content}>"
	);
}
