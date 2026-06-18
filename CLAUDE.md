# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET 8 test automation framework for UI (Selenium + Chrome) and API (HttpClient) testing using NUnit and Allure reporting. Targets the DemoQA site/API as the reference application.

## Repository

GitHub: https://github.com/Vasyl-KR/Something — remote `origin`, default branch `main`.

## Common Commands

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run all tests
dotnet test

# Run a single test by name
dotnet test --filter "FullyQualifiedName~LoginTests.LoginPage_HasCorrectTitle"

# Run tests by category tag
dotnet test --filter "TestCategory=Smoke"

# View Allure report (requires Allure CLI)
allure serve Tests
```

## Architecture

Single test project (`Tests/Tests.csproj`) with this layout:

```
Tests/
├── Config/appsettings.json       # BaseUiUrl, BaseApiUrl, HeadlessBrowser, DefaultTimeoutSeconds
├── Core/
│   ├── Drivers/                  # DriverFactory (Chrome setup) + DriverManager (thread-local IWebDriver)
│   ├── Clients/ApiClient.cs      # HttpClient wrapper with typed GET/POST/PUT/DELETE + request/response logging
│   ├── Pages/BasePage.cs         # Base POM class with wait helpers
│   ├── Fixtures/
│   │   ├── UiTestFixture.cs      # Base class: creates driver on [SetUp], screenshots + quits on [TearDown]
│   │   └── ApiTestFixture.cs     # Base class: creates ApiClient on [OneTimeSetUp]
│   └── Helpers/
│       ├── ConfigReader.cs       # Static typed access to appsettings.json values
│       ├── WaitHelper.cs         # Explicit WebDriverWait wrappers
│       └── Log.cs                # Serilog wrapper — console + rolling daily file (logs/test-run-YYYYMMDD.log)
├── UI/
│   ├── Pages/                    # Page Object Model classes (extend BasePage)
│   └── Tests/                    # UI test classes (extend UiTestFixture)
└── API/
    ├── Models/                   # Newtonsoft.Json-annotated DTOs
    └── Tests/                    # API test classes (extend ApiTestFixture)
```

### Key conventions

- **UI tests** inherit `UiTestFixture` — driver is available via `Driver` property, no manual setup needed.
- **API tests** inherit `ApiTestFixture` — `HttpClient` wrapper is available via `Api` property.
- **Page classes** inherit `BasePage` and receive `IWebDriver` via constructor. Use `WaitForElement(By)` / `WaitForClickable(By)` rather than direct `FindElement` calls.
- **Logging** — use `Log.Info()`, `Log.Debug()`, `Log.Warning()`, `Log.Error()` anywhere in tests or pages. Both fixtures log test start/end and outcome automatically. `ApiClient` logs every request and response status code.
- **Config** is read via `ConfigReader` static properties; update `Config/appsettings.json` to point at a different environment.
- **Allure attributes** (`[AllureSuite]`, `[AllureFeature]`, `[AllureTag]`, `[AllureName]`) go on test classes/methods. The `[AllureNUnit]` attribute on fixture base classes wires up the adapter automatically. Allure results are written to `Tests`.
- **ChromeDriver** is auto-managed by `WebDriverManager` using `MatchingBrowser` strategy — no manual driver binary needed.
