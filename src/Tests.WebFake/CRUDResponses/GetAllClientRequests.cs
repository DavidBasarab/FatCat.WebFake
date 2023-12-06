using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.Models;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class GetAllClientRequests : WebFakeEndpointTests<GetEndpoint>
{
	private readonly List<ClientRequestCacheItem> cacheItems = Faker.Create<List<ClientRequestCacheItem>>();

	private string GetAllRequestsPath
	{
		get => $"/{fakeId}/request";
	}

	public GetAllClientRequests()
	{
		endpoint = new GetEndpoint(cache, settings, thread, clientRequestCache, generator, dateTimeUtilities);

		A.CallTo(() => clientRequestCache.GetAll()).ReturnsLazily(() => cacheItems);

		SetRequestOnEndpoint(string.Empty, GetAllRequestsPath);
	}

	[Fact]
	public async Task GetAllClientRequestItems()
	{
		await endpoint.DoAction();

		A.CallTo(() => clientRequestCache.GetAll()).MustHaveHappened();
	}

	[Fact]
	public void ReturnAllClientItems()
	{
		endpoint.DoAction().Should().BeOk().BeEquivalentTo(cacheItems.Select(i => i.Request));
	}
}
