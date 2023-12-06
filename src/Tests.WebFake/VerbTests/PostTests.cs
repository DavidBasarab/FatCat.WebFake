using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;

namespace Tests.FatCat.WebFake.VerbTests;

public class PostTests : VerbTests<PostEndpoint>
{
	protected override HttpVerb Verb
	{
		get => HttpVerb.Post;
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
