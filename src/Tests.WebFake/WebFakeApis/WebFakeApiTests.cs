using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi;
using FatCat.WebFakeApi.Models;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public abstract class WebFakeApiTests
{
	protected readonly string fakeId = Faker.RandomString();
	protected readonly Uri fakeUri = new($"http://locahlost:1776/{Faker.RandomString()}");
	protected readonly FatWebResponse fatWebResponse = Faker.Create<FatWebResponse>();
	protected readonly IWebCaller webCaller = A.Fake<IWebCaller>();
	protected readonly IWebCallerFactory webCallerFactory = A.Fake<IWebCallerFactory>();
	protected readonly WebFakeAPi webFakeApi;

	protected WebFakeApiTests()
	{
		A.CallTo(() => webCallerFactory.GetWebCaller(A<Uri>._)).Returns(webCaller);

		A.CallTo(() => webCaller.Post(A<string>._, A<EntryResponse>._)).Returns(fatWebResponse);
		A.CallTo(() => webCaller.Delete(A<string>._)).Returns(fatWebResponse);

		webFakeApi = new WebFakeAPi(fakeUri, fakeId) { WebCallerFactory = webCallerFactory };
	}

	[Fact]
	public async Task CreateWebCaller()
	{
		await DoTestAction();

		A.CallTo(() => webCallerFactory.GetWebCaller(fakeUri)).MustHaveHappened();
	}

	protected abstract Task<FatWebResponse> DoTestAction();
}
