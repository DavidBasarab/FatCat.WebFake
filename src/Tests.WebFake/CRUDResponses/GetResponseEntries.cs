using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.Models;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class GetResponseEntries : WebFakeEndpointTests<GetEndpoint>
{
	private readonly List<ResponseCacheItem> cacheItems = Faker.Create<List<ResponseCacheItem>>();

	public GetResponseEntries()
	{
		A.CallTo(() => cache.GetAll()).ReturnsLazily(() => cacheItems);

		endpoint = new GetEndpoint(cache, settings, thread, clientRequestCache);

		SetRequestOnEndpoint(string.Empty, ResponsePath);
	}

	[Fact]
	public void BeAGet()
	{
		endpoint.Should().BeGet(nameof(GetEndpoint.DoAction), "{*url}");
	}

	[Fact]
	public async Task DoNotGetFromTheCacheIfNotAResponseEntry()
	{
		SetRequestOnEndpoint(string.Empty, "/stuff");

		await endpoint.DoAction();

		A.CallTo(() => cache.GetAll()).MustNotHaveHappened();
	}

	[Fact]
	public async Task GetAllItemsFromCache()
	{
		await endpoint.DoAction();

		A.CallTo(() => cache.GetAll()).MustHaveHappened();
	}

	[Fact]
	public void IfNoItemsInCacheReturnEmptyArray()
	{
		cacheItems.Clear();

		var expectedList = new List<EntryRequest>();

		endpoint.DoAction().Should().BeOk().BeEquivalentTo(expectedList);
	}

	[Fact]
	public async Task ReadFakeId()
	{
		await endpoint.DoAction();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public void ReturnAllResponseEntries()
	{
		var expectedList = cacheItems.Select(i => i.Entry).ToList();

		endpoint.DoAction().Should().BeOk().BeEquivalentTo(expectedList);
	}
}
