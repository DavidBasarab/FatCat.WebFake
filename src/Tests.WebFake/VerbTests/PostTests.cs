using FatCat.WebFake.Endpoints;
using FatCat.WebFakeApi.Models;

namespace Tests.FatCat.WebFake.VerbTests;

public class PostTests : VerbTests<PostEndpoint>
{
	protected override HttpVerb Verb
	{
		get => HttpVerb.Post;
	}

	protected override PostEndpoint CreateEndpoint()
	{
		return new PostEndpoint(cache, settings, thread, clientRequestCache, generator, dateTimeUtilities);
	}
}
