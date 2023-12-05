using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;
using Xunit;

namespace Tests.FatCat.WebFake.VerbTests;

public class GetTests : WebFakeEndpointTests<GetEndpoint>
{
	//https://stackoverflow.com/a/46185124/2469
	private const string GetPath = "/Some/Path/That/I/Am/Getting";
	private EntryRequest entryRequest = Faker.Create<EntryRequest>();

	public GetTests()
	{
		endpoint = new GetEndpoint(cache, settings);

		SetRequestOnEndpoint(GetPath);

		A.CallTo(() => cache.Get(A<string>._))
			.ReturnsLazily(() => entryRequest is null ? null : new ResponseCacheItem { Entry = entryRequest });
	}

	[Fact]
	public async Task GetEntryRequestFromCache()
	{
		await endpoint.DoGet();

		A.CallTo(() => cache.Get(GetPath.ToLower())).MustHaveHappened();
	}

	[Fact]
	public void IfPathIsNotInCacheReturnNotFound()
	{
		entryRequest = null;

		endpoint.DoGet().Should().BeNotFound();
	}
}
