using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;

namespace FatCat.WebFakeApi;

public interface IWebFakeAPi
{
	string FakeId { get; }

	Uri FakeUri { get; }

	Task<FatWebResponse> CreateEntryRequest(EntryRequest request);

	Task<FatWebResponse> DeleteResponse(string pathToDeleteWithVerb);

	Task<FatWebResponse> DeleteResponse(HttpVerb verb, string pathToDelete);

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
		return WebCaller.Post($"{FakeId}/response", request);
	}

	public Task<FatWebResponse> DeleteResponse(string pathToDeleteWithVerb)
	{
		return WebCaller.Delete($"{FakeId}/response/{pathToDeleteWithVerb}");
	}

	public Task<FatWebResponse> DeleteResponse(HttpVerb verb, string pathToDelete)
	{
		return DeleteResponse($"{verb}-{pathToDelete}".ToLower());
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
