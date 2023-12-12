using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.WebServer.Testing;
using FatCat.WebFake.Endpoints;
using FatCat.WebFakeApi.Models;
using Xunit;

namespace Tests.FatCat.WebFake.CRUDResponses;

public class DeleteResponseEntry : WebFakeEndpointTests<DeleteEndpoint>
{
	private readonly string pathToDelete = $"/some/path/{Faker.RandomString()}";

	public DeleteResponseEntry()
	{
		var fullEndingPath = $"{ResponsePath}{pathToDelete}";

		endpoint = new DeleteEndpoint(cache, settings, thread, clientRequestCache, generator, dateTimeUtilities);

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
	public async Task DeleteTheResponse()
	{
		await endpoint.DoAction();

		var expectedPath = pathToDelete.Remove(0, 1);

		A.CallTo(() => cache.Remove(expectedPath)).MustHaveHappened();
	}

	[Fact]
	public async Task GetFakeIdFromSettings()
	{
		await endpoint.DoAction();

		A.CallTo(() => settings.FakeId).MustHaveHappened();
	}

	[Fact]
	public async Task IfStartingWithResponsePathDoNothing()
	{
		SetRequestOnEndpoint(pathToDelete);

		await endpoint.DoAction();

		A.CallTo(() => cache.Remove(A<string>._)).MustNotHaveHappened();
	}
}
