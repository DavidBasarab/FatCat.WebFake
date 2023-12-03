using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class DeleteResponseEntry : WebFakeEndpointTests<DeleteEndpoint>
{
	private readonly string pathToDelete = $"/some/path/{Faker.RandomString()}";

	public DeleteResponseEntry()
	{
		var fullEndingPath = $"{ResponsePath}{pathToDelete}";

		endpoint = new DeleteEndpoint(cache, settings);

		SetRequestOnEndpoint(fullEndingPath);
	}

	[Fact]
	public void BeADelete()
	{
		endpoint.Should().BeDelete(nameof(DeleteEndpoint.ProcessDelete), "{*url}");
	}

	[Fact]
	public void BeOk()
	{
		endpoint.ProcessDelete().Should().BeOk().Be(ResponseCodes.EntryRemoved);
	}

	[Fact]
	public void DeleteTheResponse()
	{
		endpoint.ProcessDelete();

		A.CallTo(() => cache.Remove(pathToDelete)).MustHaveHappened();
	}

	[Fact]
	public void GetFakeIdFromSettings()
	{
		endpoint.ProcessDelete();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public void IfStartingWithResponsePathDoNothing()
	{
		SetRequestOnEndpoint(pathToDelete);

		endpoint.ProcessDelete();

		A.CallTo(() => cache.Remove(A<string>._)).MustNotHaveHappened();
	}
}
