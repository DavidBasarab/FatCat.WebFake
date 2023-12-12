using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi;
using FatCat.WebFakeApi.Models;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public class CreateResponseTests
{
	private readonly EntryResponse entryResponse = Faker.Create<EntryResponse>();
	private readonly string fakeId = Faker.RandomString();
	private readonly Uri fakeUri = new($"http://locahlost:1776/{Faker.RandomString()}");
	private readonly FatWebResponse fatWebResponse = Faker.Create<FatWebResponse>();
	private readonly IWebCaller webCaller = A.Fake<IWebCaller>();
	private readonly IWebCallerFactory webCallerFactory = A.Fake<IWebCallerFactory>();
	private readonly WebFakeAPi webFakeApi;

	public CreateResponseTests()
	{
		A.CallTo(() => webCallerFactory.GetWebCaller(A<Uri>._)).Returns(webCaller);

		A.CallTo(() => webCaller.Post(A<string>._, A<EntryResponse>._)).Returns(fatWebResponse);

		webFakeApi = new WebFakeAPi(fakeUri, fakeId) { WebCallerFactory = webCallerFactory };
	}

	[Fact]
	public async Task CreateWebCaller()
	{
		await webFakeApi.CreateResponse(entryResponse);

		A.CallTo(() => webCallerFactory.GetWebCaller(fakeUri)).MustHaveHappened();
	}

	[Fact]
	public void ReturnWebCallerResponse()
	{
		webFakeApi.CreateResponse(entryResponse).Should().Be(fatWebResponse);
	}

	[Fact]
	public async Task SendCreateResponseToApi()
	{
		await webFakeApi.CreateResponse(entryResponse);

		A.CallTo(() => webCaller.Post($"{fakeId}/response", entryResponse)).MustHaveHappened();
	}
}
