using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.Models;
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

		endpoint = new PostEndpoint(cache, settings, thread, clientRequestCache, generator, dateTimeUtilities);

		SetRequestOnEndpoint(string.Empty, "/stuff");
	}

	[Fact]
	public async Task AddEntryToCache()
	{
		SetUpEntryRequest();

		await endpoint.DoAction();

		var requestCopy = entryRequest.DeepCopy();

		requestCopy.Path = requestCopy.Path.ToLower();

		var expectedCacheItem = new ResponseCacheItem { Entry = requestCopy };

		A.CallTo(() => cache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public void BeAPost()
	{
		endpoint.Should().BePost(nameof(PostEndpoint.DoAction), "{*url}");
	}

	[Fact]
	public async Task CanCreateAnEntryWithSamePathButDifferentVerbs()
	{
		entryRequest.Verb = HttpVerb.Post;

		SetUpEntryRequest();

		await endpoint.DoAction();

		var requestCopy = entryRequest.DeepCopy();

		requestCopy.Path = requestCopy.Path.ToLower();

		var expectedCacheItem = new ResponseCacheItem { Entry = requestCopy };

		A.CallTo(() => cache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public async Task CheckIfEntryInCache()
	{
		SetUpEntryRequest();

		await endpoint.DoAction();

		A.CallTo(() => cache.InCache($"{entryRequest.Verb}-{entryRequest.Path.ToLower()}")).MustHaveHappened();
	}

	[Fact]
	public async Task GetWebFakeId()
	{
		await endpoint.DoAction();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public async Task IfEndpointPathDoesNotStartWithFakeIdDoNotAddToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), "/response");

		await endpoint.DoAction();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public async Task IfEntryAlreadyExistsInCacheDoNothing()
	{
		SetUpEntryRequest();
		inCache = true;

		await endpoint.DoAction();

		A.CallTo(() => cache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public void IfEntryAlreadyInCacheReturnBadRequest()
	{
		SetUpEntryRequest();
		inCache = true;

		endpoint.DoAction().Should().BeBadRequest("entry-already-exists");
	}

	[Fact]
	public void PathMustStartWithForwardSlash()
	{
		entryRequest.Path = "path/has/no/forward/slash";

		SetUpEntryRequest();

		endpoint.DoAction().Should().BeBadRequest(ResponseCodes.PathMustStartWithSlash);
	}

	[Fact]
	public void ReturnOkayIfAdded()
	{
		SetUpEntryRequest();

		endpoint.DoAction().Should().BeOk();
	}

	private void SetUpEntryRequest()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), ResponsePath);
	}
}
