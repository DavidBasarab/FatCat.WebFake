using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using Xunit;

namespace Tests.FatCat.WebFake;

public class GetResponseEntries : CatchAllEndpointTests<CatchAllGetEndpoint>
{
	private readonly List<ResponseCacheItem> cacheItems = Faker.Create<List<ResponseCacheItem>>();

	public GetResponseEntries()
	{
		A.CallTo(() => cache.GetAll()).ReturnsLazily(() => cacheItems);

		endpoint = new CatchAllGetEndpoint(cache, webFakeSettings);
	}

	[Fact]
	public void BeAGet()
	{
		endpoint.Should().BeGet(nameof(CatchAllGetEndpoint.ProcessGet), "{*url}");
	}
}
