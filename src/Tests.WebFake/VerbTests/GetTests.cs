using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;

namespace Tests.FatCat.WebFake.VerbTests;

public class GetTests : VerbTests<GetEndpoint>
{
	protected override HttpVerb Verb
	{
		get => HttpVerb.Get;
	}

	protected override GetEndpoint CreateEndpoint()
	{
		return new GetEndpoint(cache, settings, thread);
	}

	protected override async Task<WebResult> ExecuteEndpointAction()
	{
		return await endpoint.DoGet();
	}
}
