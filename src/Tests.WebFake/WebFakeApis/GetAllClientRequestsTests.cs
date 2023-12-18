using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public class GetAllClientRequestsTests : WebFakeApiTests
{
	private readonly List<ClientRequest> clientRequests = Faker.Create<List<ClientRequest>>();

	public GetAllClientRequestsTests()
	{
		A.CallTo(() => webCaller.Get(A<string>._)).Returns(FatWebResponse.Ok(clientRequests));
	}

	[Fact]
	public async Task GetClientRequestFromWebFake()
	{
		await webFakeApi.GetAllClientRequests();

		A.CallTo(() => webCaller.Get($"{fakeId}/request")).MustHaveHappened();
	}

	[Fact]
	public async Task ReturnClientRequests()
	{
		var result = await webFakeApi.GetAllClientRequests();

		result.Data.Should().BeEquivalentTo(clientRequests);
	}

	protected override async Task<FatWebResponse> DoTestAction()
	{
		var result = await webFakeApi.GetAllClientRequests();

		return result.BaseResponse;
	}
}
