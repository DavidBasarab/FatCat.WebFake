using FatCat.Toolkit;

namespace Tests.FatCat.WebFake;

public class TestModel : EqualObject
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public int SomeNumber { get; set; }
}
