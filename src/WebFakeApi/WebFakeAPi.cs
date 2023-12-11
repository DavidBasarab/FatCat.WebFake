using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;

namespace FatCat.WebFakeApi;

public interface IWebFakeAPi
{
	string FakeId { get; }

	Uri FakeUri { get; }

	Task<FatWebResponse> CreateResponse(EntryResponse response);

	Task<FatWebResponse> DeleteResponse(string pathToDelete);

	Task<FatWebResponse<ClientRequest>> GetAllClientRequests();

	Task<FatWebResponse<EntryRequest>> GetAllResponses();
}

public class WebFakeAPi(Uri fakeUri, string fakeId) : IWebFakeAPi
{
	public string FakeId { get; } = fakeId;

	public Uri FakeUri { get; } = fakeUri;

	public IWebCallerFactory WebCallerFactory { get; } =
		new WebCallerFactory(new ToolkitLogger(), new JsonOperations());

	public Task<FatWebResponse> CreateResponse(EntryResponse response)
	{
		throw new NotImplementedException();
	}

	public Task<FatWebResponse> DeleteResponse(string pathToDelete)
	{
		throw new NotImplementedException();
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
