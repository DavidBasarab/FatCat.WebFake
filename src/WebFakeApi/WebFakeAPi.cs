using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;

namespace FatCat.WebFakeApi;

public interface IWebFakeAPi
{
	string FakeId { get; }

	Uri FakeUri { get; }

	Task<FatWebResponse> CreateResponse(EntryResponse entryResponse);

	Task<FatWebResponse> DeleteResponse(string pathToDelete);

	Task<FatWebResponse<ClientRequest>> GetAllClientRequests();

	Task<FatWebResponse<EntryRequest>> GetAllResponses();
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

	public Task<FatWebResponse> CreateResponse(EntryResponse entryResponse)
	{
		return WebCaller.Post($"{FakeId}/response", entryResponse);
	}

	public Task<FatWebResponse> DeleteResponse(string pathToDelete)
	{
		return WebCaller.Delete($"{FakeId}/response/{pathToDelete}");
	}

	public Task<FatWebResponse<ClientRequest>> GetAllClientRequests()
	{
		throw new NotImplementedException();
	}

	public Task<FatWebResponse<EntryRequest>> GetAllResponses()
	{
		throw new NotImplementedException();
	}
}
