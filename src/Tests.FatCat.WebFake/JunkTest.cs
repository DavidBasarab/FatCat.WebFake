using FluentAssertions;
using Xunit;

namespace Tests.FatCat.WebFake;

public class JunkTest
{
	[Fact]
	public void JustAJunkTest()
	{
		true
			.Should()
			.BeTrue();
	}
}