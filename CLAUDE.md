# FatCat.WebFake Coding Standards

This file defines the coding standards for the FatCat.WebFake codebase.
All code you generate — in any context — must follow the rules for the relevant language below.
The goal is that AI-generated code is indistinguishable from code written by a senior member of this team.

## ⚠️ READ THIS FIRST — Before Any Work

**Before doing anything else in this repository — reading, planning, or writing code — read [`README.md`](README.md) in full.**
It is the single source of truth for what this product is and how it is built. Do not skip it, even for a "small" change.

The README tells you the things you must know before you touch a line of code:
- **Product:** FatCat.WebFake — a fake web server for testing against RESTful APIs you cannot (or do not want to) call for real. A caller registers canned responses (verb + path → status code, body, content type, headers, optional delay) under a `fakeid` route prefix (`/{fakeid}/response`), and the server replays them for any matching request while recording every inbound request (headers and body) for later inspection at `/{fakeid}/request`. All state is in-memory and lost on restart. It runs as a console app or as the `dbasarab/web-fake` Docker image, and ships a client NuGet package, `FatCat.WebFakeApi`, for driving the fake from .NET tests.
- **Tech stack:** **.NET 11 / C# 15** across every C# project. The server is ASP.NET Core hosted through **FatCat.Toolkit.WebServer** (`ToolkitWebApplication`, `Endpoint`, `WebResult`) with Autofac DI and in-memory `IFatCatCache<T>` storage; the client library uses the **FatCat.Toolkit** web caller. Tests use **xUnit**, **FakeItEasy**, **FatCat.Fakes**, and **FluentAssertions**. There is no UI, no database, and no TypeScript in this repo.
- **Architecture:** `WebFake` is the server (one catch-all endpoint per HTTP verb deriving from `WebFakeEndpoint`); `WebFakeApi` holds the shared request/response models and the `WebFakeAPi` client and is the published NuGet package; `Tests.WebFake` covers both; `OneOff` is a hand-run console app against a running fake.
- **Standing rules:** `FatCat.WebFakeApi` is a published package — changing a public model or `IWebFakeAPi` member breaks consumers, and its version is stepped deliberately in `VersionPrefix` in `WebFakeApi.csproj`. The models in `WebFakeApi` are the wire contract between the client and the server, so a change to one must be made on both sides in the same change.

If anything below in the coding standards appears to conflict with the README, the README describes *what* the system is; these rules describe *how* to write code for it — follow both. Where the README and the actual code disagree, **the code is authoritative** — trust the code and flag the stale README.

## Repository Layout

| Path | Project | Purpose |
|---|---|---|
| `src/WebFake` | `FatCat.WebFake` | The fake web server: verb endpoints (`Endpoints/`), response/request cache items, `WebFakeSettings`, `WebFakeModule`, `Dockerfile` |
| `src/WebFakeApi` | `FatCat.WebFakeApi` | The NuGet client package: `WebFakeAPi` and the shared models (`EntryRequest`, `EntryResponse`, `ClientRequest`, `HttpVerb`, `ResponseCodes`) |
| `src/Tests.WebFake` | `Tests.FatCat.WebFake` | xUnit tests for the server endpoints and the client |
| `src/OneOff` | `OneOff` | Console app for manually exercising a running fake; not shipped |
| `src/Scripts` | — | PowerShell helpers to build, push, run, and remove the Docker image/container |

All production namespaces start with `FatCat.WebFake.*` (server) or `FatCat.WebFakeApi.*` (client package); test namespaces start with `Tests.FatCat.WebFake.*`.

Everything must build and every test must pass before work is considered done:
```bash
dotnet build src/WebFake.slnx
dotnet test src/WebFake.slnx
```

---

## C# Rules

Apply these rules to all C# code. Do not apply them to TypeScript, PowerShell, or any other language.

@.claude/rules/csharp/naming-and-structure.md
@.claude/rules/csharp/types-and-di.md
@.claude/rules/csharp/toolchain.md
@.claude/rules/csharp/async.md
@.claude/rules/csharp/errors-and-logging.md
@.claude/rules/csharp/testing.md
@.claude/rules/csharp/not-allowed.md

## PowerShell Rules

Apply these rules to all PowerShell scripts. Do not apply them to C# or any other language.

@.claude/rules/powershell/powershell.md
