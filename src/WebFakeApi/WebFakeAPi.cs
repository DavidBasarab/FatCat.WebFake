using FatCat.Toolkit.Console;
using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;
using Newtonsoft.Json;

namespace FatCat.WebFakeApi;

public interface IWebFakeAPi
{
	string FakeId { get; }

	Uri FakeUri { get; }

	Task<FatWebResponse> CreateEntryRequest(EntryRequest request);

	Task<FatWebResponse> DeleteResponse(string pathToDelete);

	Task<FatWebResponse<List<ClientRequest>>> GetAllClientRequests();

	Task<FatWebResponse<List<EntryRequest>>> GetAllResponses();
}

public class WebFakeAPi(Uri fakeUri, string fakeId) : IWebFakeAPi
{
	private IWebCaller webCaller;

	public string FakeId { get; } = fakeId;

	public Uri FakeUri { get; } = fakeUri;

	public IWebCallerFactory WebCallerFactory { get; set; } =
		new WebCallerFactory(new ToolkitLogger(), new JsonOperations());

	private IWebCaller WebCaller
	{
		get => webCaller ??= WebCallerFactory.GetWebCaller(FakeUri);
	}

	public Task<FatWebResponse> CreateEntryRequest(EntryRequest request)
	{
		var json = JsonConvert.SerializeObject(request, Formatting.Indented);

		ConsoleLog.WriteMagenta(json);

		return WebCaller.Post($"{FakeId}/response", json);
	}

	public Task<FatWebResponse> DeleteResponse(string pathToDelete)
	{
		return WebCaller.Delete($"{FakeId}/response/{pathToDelete}");
	}

	public async Task<FatWebResponse<List<ClientRequest>>> GetAllClientRequests()
	{
		var response = await WebCaller.Get($"{FakeId}/request");

		return new FatWebResponse<List<ClientRequest>>(response);
	}

	public async Task<FatWebResponse<List<EntryRequest>>> GetAllResponses()
	{
		var response = await WebCaller.Get($"{FakeId}/response");

		return new FatWebResponse<List<EntryRequest>>(response);
	}
}
