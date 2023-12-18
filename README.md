# FatCat.WebFake
## _A fake web server that can be used for testing RESTful Apis_

A fake web server that can be used for testing RESTful Apis.  Use this when you want a way to define routes and test againist an API that you do not have access too, cost per request, or to validate other application logic.

A good use case is that you are a developer who has an API defined from a 3rd party.  You don't have access to the 3rd party API but you want to ensure that the flow of your application is working.   This is why FatCat.WebFake was created.

## Features
- Define Post, Get, Put, and Deletes by path
- Get all the test entries you have defined
- See all requests including headers made to the fake for validation

## Running Fake Web Server

All API routes to control the Fake Web Server require a fake id to control the  fake server.

`http://localhost:14888/your_fake_id/response`

##### Command Line

```
dotnet run -- urls="http://localhost:14888" fakeid=your_fake_id
```

##### Docker

```
docker create -p 14888:80  -e ASPNETCORE_URLS=http://+ -e fakeid=your_fake_id --name web-fake --restart unless-stopped dbasarab/web-fake:latest

docker start web-fake
```

_Please note The fake does not presist requests.  If you want to clear all requests just restart._

## API

#### Entry Response JSON

```{
  "Response": {
    "Delay": "00:00:05",
    "Body": "{\r\n  \"Number\": 9,\r\n  \"SomeWord\": \"Bird\"\r\n}",
    "HttpStatusCode": 200,
    "ContentType": "application/json"
  },
  "Path": "/fake/path",
  "Verb": "Get"
}
```

This will create a `get` entry for path `/fake/path`.  This `get` will take `5` seconds to response and return the provided status code of `200` and with the body defined

##### Valid Verbs
- Get
- Post
- Put
- Delete

#### Create Entry

Post to `http://localhost:14888/your_fake_id/response`

#### View All Entries

Get from `http://localhost:14888/your_fake_id/response`

#### View All Requests made to the Fake Server

Get from `http://localhost:14888/your_fake_id/request`

#### Delete an Entry

Entries are deleted by `VERB-path`  to delete the example entry you would delete to the following endpoint

`http://localhost:14888/your_fake_id/response/get-/fake-path`

## Nuget Package

In `dotnet` you can use the nuget package to interact with the web fake server

`https://www.nuget.org/packages/FatCat.WebFakeApi/`

```
dotnet add package FatCat.WebFakeApi
```