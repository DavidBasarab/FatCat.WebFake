using FluentAssertions;
using FakeItEasy;
using FatCat.WebFake.Endpoints;
using Xunit;

namespace Tests.FatCat.WebFake.VerbTests;

public class GetTests : WebFakeEndpointTests<GetEndpoint>
{
	public GetTests()
	{
		endpoint = new GetEndpoint(cache, settings);
	}
}
