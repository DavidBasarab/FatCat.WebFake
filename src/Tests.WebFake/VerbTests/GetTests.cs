using FatCat.WebFake.Endpoints;
using FatCat.WebFake.Models;

namespace Tests.FatCat.WebFake.VerbTests;

public class GetTests : VerbTests<GetEndpoint>
{
	protected override HttpVerb Verb
	{
		get => HttpVerb.Get;
	}

	protected override GetEndpoint CreateEndpoint()
	{
		return new GetEndpoint(cache, settings, thread, clientRequestCache, generator, dateTimeUtilities);
	}
}
