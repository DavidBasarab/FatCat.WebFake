using System.Net;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;
using FluentAssertions;
using Humanizer;
using Newtonsoft.Json;
using Xunit;

namespace Tests.FatCat.WebFake.VerbTests;

public class GetTests : WebFakeEndpointTests<GetEndpoint>
{
	//https://stackoverflow.com/a/46185124/2469
	private const string GetPath = "/Some/Path/That/I/Am/Getting";
	private readonly EntryResponse response = Faker.Create<EntryResponse>();
	private EntryRequest entryRequest;

	public GetTests()
	{
		endpoint = new GetEndpoint(cache, settings, thread);

		entryRequest = Faker.Create<EntryRequest>(afterCreate: i => i.Response = response);

		SetUpResponse();
		SetRequestOnEndpoint(GetPath);
		SetUpCache();
	}

	[Fact]
	public async Task AddHeadersIfPopulated()
	{
		response.Headers.Add("x-fake1", "fake1");
		response.Headers.Add("x-fake34", "fake34");

		await endpoint.DoGet();

		VerifyHeader("x-fake1", "fake1");
		VerifyHeader("x-fake34", "fake34");
	}

	[Fact]
	public async Task BasicReturnStatusCodeAndResponseFromCacheItem()
	{
		var result = await endpoint.DoGet();

		result.ContentType.Should().Be("application/json; charset=UTF-8");
		result.Content.Should().Be(response.Body);
		result.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task CanChangeContentType()
	{
		response.ContentType = "text/plain";

		var result = await endpoint.DoGet();

		result.ContentType.Should().Be("text/plain");
		result.Content.Should().Be(response.Body);
		result.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task CanChangeStatusCode()
	{
		response.HttpStatusCode = HttpStatusCode.NotModified;

		var result = await endpoint.DoGet();

		result.ContentType.Should().Be("application/json; charset=UTF-8");
		result.Content.Should().Be(response.Body);
		result.StatusCode.Should().Be(HttpStatusCode.NotModified);
	}

	[Fact]
	public async Task DoNotAddHeaders()
	{
		await endpoint.DoGet();

		endpoint.Response.Headers.Should().BeEmpty();
	}

	[Fact]
	public async Task DoNotWaitIfNoDelay()
	{
		response.Delay = null;

		await endpoint.DoGet();

		A.CallTo(() => thread.Sleep(A<TimeSpan>._)).MustNotHaveHappened();
	}

	[Fact]
	public async Task GetEntryRequestFromCache()
	{
		await endpoint.DoGet();

		A.CallTo(() => cache.Get(GetPath.ToLower())).MustHaveHappened();
	}

	[Fact]
	public void IfPathIsNotInCacheReturnNotFound()
	{
		entryRequest = null;

		endpoint.DoGet().Should().BeNotFound();
	}

	[Fact]
	public async Task WaitIfDelayIsSet()
	{
		response.Delay = 3.Seconds();

		await endpoint.DoGet();

		A.CallTo(() => thread.Sleep(3.Seconds())).MustHaveHappened();
	}

	private void SetUpCache()
	{
		A.CallTo(() => cache.Get(A<string>._))
		.ReturnsLazily(() => entryRequest is null ? null : new ResponseCacheItem { Entry = entryRequest });
	}

	private void SetUpResponse()
	{
		var model = Faker.Create<TestModel>();

		response.Headers.Clear();
		response.ContentType = "application/json; charset=UTF-8";
		response.Body = JsonConvert.SerializeObject(model);
		response.HttpStatusCode = HttpStatusCode.OK;
	}

	private void VerifyHeader(string name, string value)
	{
		var foundValue = endpoint.Response.Headers.TryGetValue(name, out var item);

		foundValue.Should().BeTrue();

		item[0].Should().Be(value);
	}
}