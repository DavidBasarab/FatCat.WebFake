using FluentAssertions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FatCat.Toolkit.Web;
using FatCat.WebFakeApi.Models;
using Xunit;

namespace Tests.FatCat.WebFake.WebFakeApis;

public class DeleteResponseWithVerbTests : WebFakeApiTests
{
	private readonly string pathToDelete = $"path/{Faker.RandomString()}";

	[Fact]
	public async Task DeletePathToWebFake()
	{
		await DoTestAction();

		A.CallTo(() => webCaller.Delete($"{fakeId}/response/post-{pathToDelete}")).MustHaveHappened();
	}

	[Fact]
	public void ReturnFatWebResponse()
	{
		DoTestAction().Should().Be(fatWebResponse);
	}

	protected override Task<FatWebResponse> DoTestAction()
	{
		return webFakeApi.DeleteResponse(HttpVerb.Post, pathToDelete);
	}
}
