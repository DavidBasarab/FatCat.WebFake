using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public class GetAllResponsesTests : WebFakeApiTests
{
	private readonly List<EntryRequest> entryRequests = Faker.Create<List<EntryRequest>>();

	public GetAllResponsesTests()
	{
		A.CallTo(() => webCaller.Get(A<string>._)).Returns(FatWebResponse.Ok(entryRequests));
	}

	[Fact]
	public async Task GetClientRequestFromWebFake()
	{
		await webFakeApi.GetAllResponses();

		A.CallTo(() => webCaller.Get($"{fakeId}/response")).MustHaveHappened();
	}

	[Fact]
	public async Task ReturnAllResponses()
	{
		var result = await webFakeApi.GetAllResponses();

		result.Data.Should().BeEquivalentTo(entryRequests);
	}

	protected override async Task<FatWebResponse> DoTestAction()
	{
		var result = await webFakeApi.GetAllResponses();

		return result.BaseResponse;
	}
}
