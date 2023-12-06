using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.Models;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class DeleteResponseEntry : WebFakeEndpointTests<DeleteEndpoint>
{
	private readonly string pathToDelete = $"/some/path/{Faker.RandomString()}";

	public DeleteResponseEntry()
	{
		var fullEndingPath = $"{ResponsePath}{pathToDelete}";

		endpoint = new DeleteEndpoint(cache, settings, thread);

		SetRequestOnEndpoint(fullEndingPath);
	}

	[Fact]
	public void BeADelete()
	{
		endpoint.Should().BeDelete(nameof(DeleteEndpoint.DoAction), "{*url}");
	}

	[Fact]
	public void BeOk()
	{
		endpoint.DoAction().Should().BeOk().Be(ResponseCodes.EntryRemoved);
	}

	[Fact]
	public void DeleteTheResponse()
	{
		endpoint.DoAction();

		A.CallTo(() => cache.Remove(pathToDelete)).MustHaveHappened();
	}

	[Fact]
	public void GetFakeIdFromSettings()
	{
		endpoint.DoAction();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public void IfStartingWithResponsePathDoNothing()
	{
		SetRequestOnEndpoint(pathToDelete);

		endpoint.DoAction();

		A.CallTo(() => cache.Remove(A<string>._)).MustNotHaveHappened();
	}
}
