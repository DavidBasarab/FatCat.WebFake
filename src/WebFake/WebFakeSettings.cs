using Microsoft.Extensions.Configuration;

namespace FatCat.WebFake;

public interface IWebFakeSettings
{
	string FakeId { get; }
}

public class WebFakeSettings(IConfiguration configuration) : IWebFakeSettings
{
	public string FakeId
	{
		get => configuration["FakeId"];
	}
}
