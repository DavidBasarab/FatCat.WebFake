using System.Net;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.ServiceModels;
using FluentAssertions;
using Humanizer;
using Newtonsoft.Json;
using Xunit;

namespace Tests.FatCat.WebFake.VerbTests;

public abstract class VerbTests<TEndpoint> : WebFakeEndpointTests<TEndpoint>
	where TEndpoint : WebFakeEndpoint
{
	private const string Path = "/Some/Path/That/I/Am/Getting";
	private readonly EntryResponse response = Faker.Create<EntryResponse>();
	private EntryRequest entryRequest;

	protected abstract HttpVerb Verb { get; }

	protected VerbTests()
	{
		// ReSharper disable once VirtualMemberCallInConstructor
		endpoint = CreateEndpoint();

		entryRequest = Faker.Create<EntryRequest>(afterCreate: i => i.Response = response);

		SetUpResponse();
		SetRequestOnEndpoint(Path);
		SetUpCache();
	}

	[Fact]
	public async Task AddHeadersIfPopulated()
	{
		response.Headers.Add("x-fake1", "fake1");
		response.Headers.Add("x-fake34", "fake34");

		await ExecuteEndpointAction();

		VerifyHeader("x-fake1", "fake1");
		VerifyHeader("x-fake34", "fake34");
	}

	[Fact]
	public async Task BasicReturnStatusCodeAndResponseFromCacheItem()
	{
		var result = await ExecuteEndpointAction();

		result.ContentType.Should().Be("application/json; charset=UTF-8");
		result.Content.Should().Be(response.Body);
		result.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task CanChangeContentType()
	{
		response.ContentType = "text/plain";

		var result = await ExecuteEndpointAction();

		result.ContentType.Should().Be("text/plain");
		result.Content.Should().Be(response.Body);
		result.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task CanChangeStatusCode()
	{
		response.HttpStatusCode = HttpStatusCode.NotModified;

		var result = await ExecuteEndpointAction();

		result.ContentType.Should().Be("application/json; charset=UTF-8");
		result.Content.Should().Be(response.Body);
		result.StatusCode.Should().Be(HttpStatusCode.NotModified);
	}

	[Fact]
	public async Task DoNotAddHeaders()
	{
		await ExecuteEndpointAction();

		endpoint.Response.Headers.Should().BeEmpty();
	}

	[Fact]
	public async Task DoNotWaitIfNoDelay()
	{
		response.Delay = null;

		await ExecuteEndpointAction();

		A.CallTo(() => thread.Sleep(A<TimeSpan>._)).MustNotHaveHappened();
	}

	[Fact]
	public async Task GetEntryRequestFromCache()
	{
		await ExecuteEndpointAction();

		A.CallTo(() => cache.Get(Path.ToLower())).MustHaveHappened();
	}

	[Fact]
	public async Task IfPathIsNotInCacheReturnNotFound()
	{
		entryRequest = null;

		await TestEndpointReturnsNotFound();
	}

	[Fact]
	public async Task IfRequestDoesMatchHttpVerbThenReturnNotFound()
	{
		entryRequest.Verb = GetInvalidVerb();

		await TestEndpointReturnsNotFound();
	}

	[Fact]
	public async Task WaitIfDelayIsSet()
	{
		response.Delay = 3.Seconds();

		await ExecuteEndpointAction();

		A.CallTo(() => thread.Sleep(3.Seconds())).MustHaveHappened();
	}

	protected abstract TEndpoint CreateEndpoint();

	protected abstract Task<WebResult> ExecuteEndpointAction();

	private HttpVerb GetInvalidVerb()
	{
		var values = Enum.GetValues<HttpVerb>();

		foreach (var httpVerb in values)
		{
			if (httpVerb != Verb)
			{
				return httpVerb;
			}
		}

		return values.First();
	}

	private void SetUpCache()
	{
		A.CallTo(() => cache.Get(A<string>._))
			.ReturnsLazily(() => entryRequest is null ? null : new ResponseCacheItem { Entry = entryRequest });
	}

	private void SetUpResponse()
	{
		var model = Faker.Create<TestModel>();

		entryRequest.Verb = Verb;

		response.Headers.Clear();
		response.ContentType = "application/json; charset=UTF-8";
		response.Body = JsonConvert.SerializeObject(model);
		response.HttpStatusCode = HttpStatusCode.OK;
	}

	private async Task TestEndpointReturnsNotFound()
	{
		var result = await ExecuteEndpointAction();

		result.Should().BeNotFound();
	}

	private void VerifyHeader(string name, string value)
	{
		var foundValue = endpoint.Response.Headers.TryGetValue(name, out var item);

		foundValue.Should().BeTrue();

		item[0].Should().Be(value);
	}
}
