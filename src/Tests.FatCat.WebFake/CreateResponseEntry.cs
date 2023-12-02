using System.Text;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace Tests.FatCat.WebFake;

public class CreateResponseEntry
{
	private readonly CatchAllPostEndpoint endpoint;

	private readonly EntryRequest entryRequest = Faker.Create<EntryRequest>(
																			afterCreate: i => i.Path = Faker.RandomString("UpperPath")
																			);

	private readonly string fakeId = Faker.RandomString();
	private readonly IFatCatCache<ResponseCacheItem> responseCache = A.Fake<IFatCatCache<ResponseCacheItem>>();
	private readonly IWebFakeSettings webFakeSettings = A.Fake<IWebFakeSettings>();
	private bool inCache;

	public CreateResponseEntry()
	{
		A.CallTo(() => webFakeSettings.FakeId).ReturnsLazily(() => fakeId);

		A.CallTo(() => responseCache.InCache(A<string>._)).ReturnsLazily(() => inCache);

		endpoint = new CatchAllPostEndpoint(responseCache, webFakeSettings);

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

		A.CallTo(() => responseCache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public void BeAPost() { endpoint.Should().BePost(nameof(CatchAllPostEndpoint.ProcessCatchAll), "{*url}"); }

	[Fact]
	public async Task CheckIfEntryInCache()
	{
		SetUpEntryRequest();

		await endpoint.ProcessCatchAll();

		A.CallTo(() => responseCache.InCache(entryRequest.Path.ToLower())).MustHaveHappened();
	}

	[Fact]
	public async Task GetWebFakeId()
	{
		await endpoint.ProcessCatchAll();

		A.CallTo(() => webFakeSettings.FakeId).MustHaveHappened();
	}

	[Fact]
	public async Task IfEndpointPathDoesNotStartWithFakeIdDoNotAddToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), "/response");

		await endpoint.ProcessCatchAll();

		A.CallTo(() => responseCache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public async Task IfEntryAlreadyExistsInCacheDoNothing()
	{
		SetUpEntryRequest();
		inCache = true;

		await endpoint.ProcessCatchAll();

		A.CallTo(() => responseCache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	[Fact]
	public void ReturnOkayIfAdded()
	{
		SetUpEntryRequest();

		endpoint.ProcessCatchAll().Should().BeOk();
	}

	private void SetRequestOnEndpoint(string request, string endingPath)
	{
		var httpContext = new DefaultHttpContext();

		if (request.IsNotNullOrEmpty())
		{
			var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

			httpContext.Request.Body = memoryStream;
			httpContext.Request.ContentLength = memoryStream.Length;
			httpContext.Request.ContentType = "application/json";
		}

		httpContext.Request.Scheme = "http";
		httpContext.Request.Host = new HostString("localhost", 5000);
		httpContext.Request.PathBase = new PathString(endingPath);

		var controllerContext = new ControllerContext { HttpContext = httpContext };

		endpoint.ControllerContext = controllerContext;
	}

	private void SetUpEntryRequest() { SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), $"/{fakeId}/response"); }
}