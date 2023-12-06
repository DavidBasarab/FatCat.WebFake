using FatCat.Toolkit.WebServer;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;

namespace Tests.FatCat.WebFake.VerbTests;

public class DeleteTests : VerbTests<DeleteEndpoint>
{
	protected override HttpVerb Verb
	{
		get => HttpVerb.Delete;
	}

	protected override DeleteEndpoint CreateEndpoint()
	{
		return new DeleteEndpoint(cache, settings, thread);
	}
}
