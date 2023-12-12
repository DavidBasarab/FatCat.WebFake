using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public class CreateResponseTests : WebFakeApiTests
{
	private readonly EntryResponse entryResponse = Faker.Create<EntryResponse>();

	protected override Task<FatWebResponse> DoTestAction()
	{
		return webFakeApi.CreateResponse(entryResponse);
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
