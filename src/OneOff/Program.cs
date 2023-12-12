using System.Net;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi;
using FatCat.WebFakeApi.Models;

public static class Program
{
	private const string TestPath = "asus/rog-strix-rtx3080-10g-gaming";
	private static WebFakeAPi api;
	private static readonly Uri fakeUri = new("http://localhost:14888");

	public static async Task Main(params string[] args)
	{
		api = new WebFakeAPi(fakeUri, "david");

		var apiDeleteResponse = await api.DeleteResponse($"{HttpVerb.Delete}-{TestPath}");

		PrintResponse(apiDeleteResponse);

		var entryRequest = new EntryRequest
		{
			Path = $"/{TestPath}",
			Verb = HttpVerb.Get,
			Response = new EntryResponse { Body = "HELLO WORLD", HttpStatusCode = HttpStatusCode.OK }
		};

		await CreateEntry(entryRequest);

		entryRequest.Verb = HttpVerb.Delete;
		entryRequest.Response.Body = "GOODBYE WORLD";

		await CreateEntry(entryRequest);

		var webCallerFactory = new WebCallerFactory(new ToolkitLogger(), new JsonOperations());

		var webCaller = webCallerFactory.GetWebCaller(fakeUri);

		var response = await webCaller.Get(TestPath);

		PrintResponse(response);

		var deleteResponse = await webCaller.Delete(TestPath);

		PrintResponse(deleteResponse);
	}

	private static async Task CreateEntry(EntryRequest entryRequest)
	{
		var createResponse = await api.CreateEntryRequest(entryRequest);

		if (createResponse.IsUnsuccessful)
		{
			ConsoleLog.WriteRed(
				$"Could not create entry.  Status code: {createResponse.StatusCode} | Content <{createResponse.Content}>"
			);
		}
	}

	private static void PrintResponse(FatWebResponse response)
	{
		if (response.IsUnsuccessful)
		{
			ConsoleLog.WriteRed(
				$"Response failed.  Status code: {response.StatusCode} | Content <{response.Content}>"
			);
		}
		else
		{
			ConsoleLog.WriteGreen(
				$"Response succeeded.  Status code: {response.StatusCode} | Content <{response.Content}>"
			);
		}
	}
}
