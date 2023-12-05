using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Endpoints;

namespace Tests.FatCat.WebFake.VerbTests;

public class GetTests : VerbTests<GetEndpoint>
{
	protected override string HttpMethod
	{
		get => "GET";
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
