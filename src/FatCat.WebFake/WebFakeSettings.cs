using Microsoft.Extensions.Configuration;

namespace FatCat.WebFake;

public interface IWebFakeSettings
{
	string UniqueId { get; }
}

public class WebFakeSettings(IConfiguration configuration) : IWebFakeSettings
{
	public string UniqueId
	{
		get => configuration["FakeId"];
	}
}
