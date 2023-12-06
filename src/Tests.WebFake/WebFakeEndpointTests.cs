using System.Text;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Threading;
using FatCat.WebFake;
using FatCat.WebFake.Endpoints;
using FatCat.WebFake.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tests.FatCat.WebFake;

public class WebFakeEndpointTests<TEndpoint>
	where TEndpoint : WebFakeEndpoint
{
	protected readonly IFatCatCache<ResponseCacheItem> cache = A.Fake<IFatCatCache<ResponseCacheItem>>();

	protected readonly IFatCatCache<ClientRequestCacheItem> clientRequestCache = A.Fake<
		IFatCatCache<ClientRequestCacheItem>
	>();

	protected readonly string fakeId = Faker.RandomString();
	protected readonly IGenerator generator = A.Fake<IGenerator>();
	protected readonly IWebFakeSettings settings = A.Fake<IWebFakeSettings>();
	protected readonly IThread thread = A.Fake<IThread>();

	protected TEndpoint endpoint;

	protected string ResponsePath
	{
		get => $"/{fakeId}/response";
	}

	protected WebFakeEndpointTests()
	{
		A.CallTo(() => settings.FakeId).ReturnsLazily(() => fakeId);
	}

	protected void SetRequestOnEndpoint(string endingPath)
	{
		SetRequestOnEndpoint(string.Empty, endingPath);
	}

	protected void SetRequestOnEndpoint(string request, string endingPath)
	{
		var httpContext = new DefaultHttpContext();

		if (request.IsNotNullOrEmpty())
		{
			var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

			httpContext.Request.Body = memoryStream;
			httpContext.Request.ContentLength = memoryStream.Length;
			httpContext.Request.ContentType = "application/json";
		}

		httpContext.Request.Scheme = "http";
		httpContext.Request.Host = new HostString("localhost", 5000);
		httpContext.Request.PathBase = new PathString(endingPath);

		var controllerContext = new ControllerContext { HttpContext = httpContext };

		endpoint.ControllerContext = controllerContext;
	}
}
