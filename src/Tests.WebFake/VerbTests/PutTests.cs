﻿using FatCat.WebFake.Endpoints;
using FatCat.WebFakeApi.Models;

namespace Tests.FatCat.WebFake.VerbTests;

public class PutTests : VerbTests<PutEndpoint>
{
	protected override HttpVerb Verb
	{
		get => HttpVerb.Put;
	}

	protected override PutEndpoint CreateEndpoint()
	{
		return new PutEndpoint(cache, settings, thread, clientRequestCache, generator, dateTimeUtilities);
	}
}
