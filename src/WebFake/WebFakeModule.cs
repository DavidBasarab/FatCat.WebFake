using Autofac;
using FatCat.Toolkit.Caching;

namespace FatCat.WebFake;

public class WebFakeModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<FatCatCache<ResponseCacheItem>>()
			.As<IFatCatCache<ResponseCacheItem>>()
			.SingleInstance();
	}
}
