using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Endpoints;

namespace Tests.FatCat.WebFake.VerbTests;

public class PostTests : VerbTests<PostEndpoint>
{
	protected override string HttpMethod
	{
		get => "POST";
	}

	protected override PostEndpoint CreateEndpoint()
	{
		return new PostEndpoint(cache, settings, thread);
	}

	protected override async Task<WebResult> ExecuteEndpointAction()
	{
		return await endpoint.DoPost();
	}
}
