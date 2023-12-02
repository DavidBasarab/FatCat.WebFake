using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.ServiceModels;
using Newtonsoft.Json;
using Xunit;

namespace Tests.FatCat.WebFake;

public class CreateResponseEntry : CatchAllEndpointTests<CatchAllPostEndpoint>
{
	private readonly EntryRequest entryRequest = Faker.Create<EntryRequest>(
		afterCreate: i => i.Path = Faker.RandomString("UpperPath")
	);

	private bool inCache;

	public CreateResponseEntry()
	{
		A.CallTo(() => cache.InCache(A<string>._)).ReturnsLazily(() => inCache);

		endpoint = new CatchAllPostEndpoint(cache, settings);

		SetRequestOnEndpoint(string.Empty, "/stuff");
	}

	[Fact]
	public async Task AddEntryToCache()
	{
		SetUpEntryRequest();

		await endpoint.ProcessCatchAll();

		var requestCopy = entryRequest.DeepCopy();

		requestCopy.Path = requestCopy.Path.ToLower();

		var expectedCacheItem = new ResponseCacheItem { Entry = requestCopy };

		A.CallTo(() => cache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public void BeAPost()
	{
		endpoint.Should().BePost(nameof(CatchAllPostEndpoint.ProcessCatchAll), "{*url}");
	}

	[Fact]
	public async Task CheckIfEntryInCache()
	{
		SetUpEntryRequest();

		await endpoint.ProcessCatchAll();

		A.CallTo(() => cache.InCache(entryRequest.Path.ToLower())).MustHaveHappened();
	}

	[Fact]
	public async Task GetWebFakeId()
	{
		await endpoint.ProcessCatchAll();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public async Task IfEndpointPathDoesNotStartWithFakeIdDoNotAddToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), "/response");

		await endpoint.ProcessCatchAll();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public async Task IfEntryAlreadyExistsInCacheDoNothing()
	{
		SetUpEntryRequest();
		inCache = true;

		await endpoint.ProcessCatchAll();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public void IfEntryAlreadyInCacheReturnBadRequest()
	{
		SetUpEntryRequest();
		inCache = true;

		endpoint.ProcessCatchAll().Should().BeBadRequest("entry-already-exists");
	}

	[Fact]
	public void ReturnOkayIfAdded()
	{
		SetUpEntryRequest();

		endpoint.ProcessCatchAll().Should().BeOk();
	}

	private void SetUpEntryRequest()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), ResponsePath);
	}
}
