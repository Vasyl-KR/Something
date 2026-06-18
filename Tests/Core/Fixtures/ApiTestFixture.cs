using Allure.NUnit;
using NUnit.Framework;
using Tests.Core.Clients;
using Tests.Core.Helpers;

namespace Tests.Core.Fixtures;

public abstract class ApiTestFixture
{
    protected ApiClient Api { get; private set; } = null!;

    [OneTimeSetUp]
    public void InitClient()
    {
        Log.Info($"[API] Suite START: {GetType().Name}");
        Api = new ApiClient(ConfigReader.BaseApiUrl);
        Log.Debug($"ApiClient initialized with base URL: {ConfigReader.BaseApiUrl}");
    }

    [SetUp]
    public void LogTestStart()
    {
        Log.Info($"[API] START: {TestContext.CurrentContext.Test.FullName}");
    }

    [TearDown]
    public void LogTestEnd()
    {
        var result = TestContext.CurrentContext.Result.Outcome.Status;
        Log.Info($"[API] END: {TestContext.CurrentContext.Test.Name} — {result}");

        if (result == NUnit.Framework.Interfaces.TestStatus.Failed)
            Log.Warning($"Test failed: {TestContext.CurrentContext.Result.Message}");
    }

    [OneTimeTearDown]
    public void DisposeClient()
    {
        Log.Info($"[API] Suite END: {GetType().Name}");
        Api.Dispose();
    }
}
