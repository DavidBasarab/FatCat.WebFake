using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class GetResponseEntries : WebFakeEndpointTests<GetEndpoint>
{
	private readonly List<ResponseCacheItem> cacheItems = Faker.Create<List<ResponseCacheItem>>();

	public GetResponseEntries()
	{
		A.CallTo(() => cache.GetAll()).ReturnsLazily(() => cacheItems);

		endpoint = new GetEndpoint(cache, settings);

		SetRequestOnEndpoint(string.Empty, ResponsePath);
	}

	[Fact]
	public void BeAGet()
	{
		endpoint.Should().BeGet(nameof(GetEndpoint.DoGet), "{*url}");
	}

	[Fact]
	public async Task DoNotGetFromTheCacheIfNotAResponseEntry()
	{
		SetRequestOnEndpoint(string.Empty, "/stuff");

		await endpoint.DoGet();

		A.CallTo(() => cache.GetAll()).MustNotHaveHappened();
	}

	[Fact]
	public async Task GetAllItemsFromCache()
	{
		await endpoint.DoGet();

		A.CallTo(() => cache.GetAll()).MustHaveHappened();
	}

	[Fact]
	public void IfNoItemsInCacheReturnEmptyArray()
	{
		cacheItems.Clear();

		var expectedList = new List<EntryRequest>();

		endpoint.DoGet().Should().BeOk().BeEquivalentTo(expectedList);
	}

	[Fact]
	public async Task ReadFakeId()
	{
		await endpoint.DoGet();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public void ReturnAllResponseEntries()
	{
		var expectedList = cacheItems.Select(i => i.Entry).ToList();

		endpoint.DoGet().Should().BeOk().BeEquivalentTo(expectedList);
	}
}
