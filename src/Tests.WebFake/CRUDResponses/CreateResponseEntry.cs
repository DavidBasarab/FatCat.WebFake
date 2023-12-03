using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;
using Newtonsoft.Json;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class CreateResponseEntry : WebFakeEndpointTests<PostEndpoint>
{
	private readonly EntryRequest entryRequest = Faker.Create<EntryRequest>(
		afterCreate: i => i.Path = Faker.RandomString("/UpperPath")
	);

	private bool inCache;

	public CreateResponseEntry()
	{
		A.CallTo(() => cache.InCache(A<string>._)).ReturnsLazily(() => inCache);

		endpoint = new PostEndpoint(cache, settings);

		SetRequestOnEndpoint(string.Empty, "/stuff");
	}

	[Fact]
	public async Task AddEntryToCache()
	{
		SetUpEntryRequest();

		await endpoint.ProcessPost();

		var requestCopy = entryRequest.DeepCopy();

		requestCopy.Path = requestCopy.Path.ToLower();

		var expectedCacheItem = new ResponseCacheItem { Entry = requestCopy };

		A.CallTo(() => cache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public void BeAPost()
	{
		endpoint.Should().BePost(nameof(PostEndpoint.ProcessPost), "{*url}");
	}

	[Fact]
	public async Task CheckIfEntryInCache()
	{
		SetUpEntryRequest();

		await endpoint.ProcessPost();

		A.CallTo(() => cache.InCache(entryRequest.Path.ToLower())).MustHaveHappened();
	}

	[Fact]
	public async Task GetWebFakeId()
	{
		await endpoint.ProcessPost();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public async Task IfEndpointPathDoesNotStartWithFakeIdDoNotAddToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), "/response");

		await endpoint.ProcessPost();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public async Task IfEntryAlreadyExistsInCacheDoNothing()
	{
		SetUpEntryRequest();
		inCache = true;

		await endpoint.ProcessPost();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public void IfEntryAlreadyInCacheReturnBadRequest()
	{
		SetUpEntryRequest();
		inCache = true;

		endpoint.ProcessPost().Should().BeBadRequest("entry-already-exists");
	}

	[Fact]
	public void PathMustStartWithForwardSlash()
	{
		entryRequest.Path = "path/has/no/forward/slash";

		SetUpEntryRequest();

		endpoint.ProcessPost().Should().BeBadRequest(ResponseCodes.PathMustStartWithSlash);
	}

	[Fact]
	public void ReturnOkayIfAdded()
	{
		SetUpEntryRequest();

		endpoint.ProcessPost().Should().BeOk();
	}

	private void SetUpEntryRequest()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), ResponsePath);
	}
}
