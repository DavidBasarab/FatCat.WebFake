using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;
using Xunit;

namespace Tests.FatCat.WebFake;

public class GetResponseEntries : CatchAllEndpointTests<CatchAllGetEndpoint>
{
	private readonly List<ResponseCacheItem> cacheItems = Faker.Create<List<ResponseCacheItem>>();

	public GetResponseEntries()
	{
		A.CallTo(() => cache.GetAll()).ReturnsLazily(() => cacheItems);

		endpoint = new CatchAllGetEndpoint(cache, settings);

		SetRequestOnEndpoint(string.Empty, ResponsePath);
	}

	[Fact]
	public void BeAGet()
	{
		endpoint.Should().BeGet(nameof(CatchAllGetEndpoint.ProcessGet), "{*url}");
	}

	[Fact]
	public void DoNotGetFromTheCacheIfNotAResponseEntry()
	{
		SetRequestOnEndpoint(string.Empty, "/stuff");

		endpoint.ProcessGet();

		A.CallTo(() => cache.GetAll()).MustNotHaveHappened();
	}

	[Fact]
	public void GetAllItemsFromCache()
	{
		endpoint.ProcessGet();

		A.CallTo(() => cache.GetAll()).MustHaveHappened();
	}

	[Fact]
	public void ReadFakeId()
	{
		endpoint.ProcessGet();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public void IfNoItemsInCacheReturnEmptyArray()
	{
		cacheItems.Clear();

		var expectedList = new List<EntryRequest>();

		endpoint.ProcessGet().Should().BeOk().BeEquivalentTo(expectedList);
	}

	[Fact]
	public void ReturnAllResponseEntries()
	{
		var expectedList = cacheItems.Select(i => i.Entry).ToList();

		endpoint.ProcessGet().Should().BeOk().BeEquivalentTo(expectedList);
	}
}
