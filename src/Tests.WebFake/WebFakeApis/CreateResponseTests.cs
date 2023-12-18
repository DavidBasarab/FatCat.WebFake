using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public class CreateResponseTests : WebFakeApiTests
{
	private readonly EntryRequest entryRequest = Faker.Create<EntryRequest>();

	[Fact]
	public void ReturnWebCallerResponse()
	{
		webFakeApi.CreateEntryRequest(entryRequest).Should().Be(fatWebResponse);
	}

	[Fact]
	public async Task SendCreateResponseToApi()
	{
		await webFakeApi.CreateEntryRequest(entryRequest);

		A.CallTo(() => webCaller.Post($"{fakeId}/response", entryRequest)).MustHaveHappened();
	}

	protected override Task<FatWebResponse> DoTestAction()
	{
		return webFakeApi.CreateEntryRequest(entryRequest);
	}
}
