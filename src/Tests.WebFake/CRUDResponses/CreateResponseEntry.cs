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
	private readonly EntryRequest entryRequest = Faker.Create<EntryRequest>(i =>
	{
		i.Path = Faker.RandomString("/UpperPath");
		i.Verb = HttpVerb.Get;
	});

	private bool inCache;

	public CreateResponseEntry()
	{
		A.CallTo(() => cache.InCache(A<string>._)).ReturnsLazily(() => inCache);

		endpoint = new PostEndpoint(cache, settings, thread);

		SetRequestOnEndpoint(string.Empty, "/stuff");
	}

	[Fact]
	public async Task AddEntryToCache()
	{
		SetUpEntryRequest();

		await endpoint.DoPost();

		var requestCopy = entryRequest.DeepCopy();

		requestCopy.Path = requestCopy.Path.ToLower();

		var expectedCacheItem = new ResponseCacheItem { Entry = requestCopy };

		A.CallTo(() => cache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public void BeAPost()
	{
		endpoint.Should().BePost(nameof(PostEndpoint.DoPost), "{*url}");
	}

	[Fact]
	public async Task CanCreateAnEntryWithSamePathButDifferentVerbs()
	{
		entryRequest.Verb = HttpVerb.Post;

		SetUpEntryRequest();

		await endpoint.DoPost();

		var requestCopy = entryRequest.DeepCopy();

		requestCopy.Path = requestCopy.Path.ToLower();

		var expectedCacheItem = new ResponseCacheItem { Entry = requestCopy };

		A.CallTo(() => cache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public async Task CheckIfEntryInCache()
	{
		SetUpEntryRequest();

		await endpoint.DoPost();

		A.CallTo(() => cache.InCache($"{entryRequest.Verb}-{entryRequest.Path.ToLower()}")).MustHaveHappened();
	}

	[Fact]
	public async Task GetWebFakeId()
	{
		await endpoint.DoPost();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public async Task IfEndpointPathDoesNotStartWithFakeIdDoNotAddToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), "/response");

		await endpoint.DoPost();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public async Task IfEntryAlreadyExistsInCacheDoNothing()
	{
		SetUpEntryRequest();
		inCache = true;

		await endpoint.DoPost();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public void IfEntryAlreadyInCacheReturnBadRequest()
	{
		SetUpEntryRequest();
		inCache = true;

		endpoint.DoPost().Should().BeBadRequest("entry-already-exists");
	}

	[Fact]
	public void PathMustStartWithForwardSlash()
	{
		entryRequest.Path = "path/has/no/forward/slash";

		SetUpEntryRequest();

		endpoint.DoPost().Should().BeBadRequest(ResponseCodes.PathMustStartWithSlash);
	}

	[Fact]
	public void ReturnOkayIfAdded()
	{
		SetUpEntryRequest();

		endpoint.DoPost().Should().BeOk();
	}

	private void SetUpEntryRequest()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), ResponsePath);
	}
}
