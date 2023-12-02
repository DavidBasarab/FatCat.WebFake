using System.Text;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Caching;
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
	private readonly EntryRequest entryRequest = Faker.Create<EntryRequest>();
	private readonly string fakeId = Faker.RandomString();
	private readonly IFatCatCache<ResponseCacheItem> responseCache = A.Fake<IFatCatCache<ResponseCacheItem>>();
	private readonly IWebFakeSettings webFakeSettings = A.Fake<IWebFakeSettings>();

	public CreateResponseEntry()
	{
		A.CallTo(() => webFakeSettings.FakeId).ReturnsLazily(() => fakeId);

		endpoint = new CatchAllPostEndpoint(responseCache, webFakeSettings);
	}

	[Fact]
	public async Task AddEntryToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), $"/{fakeId}/response");

		await endpoint.ProcessCatchAll();

		var expectedCacheItem = new ResponseCacheItem { Entry = entryRequest };

		A.CallTo(() => responseCache.Add(expectedCacheItem, default)).MustHaveHappened();
	}

	[Fact]
	public void BeAPost()
	{
		endpoint.Should().BePost(nameof(CatchAllPostEndpoint.ProcessCatchAll), "{*url}");
	}

	[Fact]
	public async Task IfEndpointPathDoesNotStartWithFakeIdDoNotAddToCache()
	{
		SetRequestOnEndpoint(JsonConvert.SerializeObject(entryRequest), "/response");

		await endpoint.ProcessCatchAll();

		A.CallTo(() => responseCache.Add(A<ResponseCacheItem>._, default)).MustNotHaveHappened();
	}

	private void SetRequestOnEndpoint(string request, string endingPath)
	{
		var httpContext = new DefaultHttpContext();

		var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

		httpContext.Request.Body = memoryStream;
		httpContext.Request.ContentLength = memoryStream.Length;
		httpContext.Request.ContentType = "application/json";

		httpContext.Request.Scheme = "http";
		httpContext.Request.Host = new HostString("localhost", 5000);
		httpContext.Request.PathBase = new PathString(endingPath);

		var controllerContext = new ControllerContext { HttpContext = httpContext };

		endpoint.ControllerContext = controllerContext;
	}
}
