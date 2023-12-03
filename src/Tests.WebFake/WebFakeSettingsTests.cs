using FakeItEasy;
using FatCat.Fakes;
using FatCat.WebFake;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests.FatCat.WebFake;

public class WebFakeSettingsTests
{
	private readonly IConfiguration configuration = A.Fake<IConfiguration>();
	private readonly string FakeId = Faker.RandomString();
	private readonly WebFakeSettings webSettings;

	public WebFakeSettingsTests()
	{
		A.CallTo(() => configuration["FakeId"]).ReturnsLazily(() => FakeId);

		webSettings = new WebFakeSettings(configuration);
	}

	[Fact]
	public void GetFakeId()
	{
		var _ = webSettings.FakeId;

		A.CallTo(() => configuration["FakeId"]).MustHaveHappened();
	}

	[Fact]
	public void ReturnFakeId() { webSettings.FakeId.Should().Be(FakeId); }
}