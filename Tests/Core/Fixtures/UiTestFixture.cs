using Allure.Net.Commons;
using Allure.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Tests.Core.Drivers;
using Tests.Core.Helpers;

namespace Tests.Core.Fixtures;

public abstract class UiTestFixture
{
    protected IWebDriver Driver => DriverManager.Driver;

    [SetUp]
    public void InitDriver()
    {
        Log.Info($"[UI] START: {TestContext.CurrentContext.Test.FullName}");

        var driver = DriverFactory.CreateDriver();
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        DriverManager.SetDriver(driver);

        Log.Debug("Browser initialized and maximized");
    }

    [TearDown]
    public void QuitDriver()
    {
        var result = TestContext.CurrentContext.Result.Outcome.Status;
        Log.Info($"[UI] END: {TestContext.CurrentContext.Test.Name} — {result}");

        if (result == TestStatus.Failed)
        {
            Log.Warning($"Test failed: {TestContext.CurrentContext.Result.Message}");
            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            AllureApi.AddAttachment("Screenshot on failure", "image/png", screenshot.AsByteArray);
        }

        DriverManager.QuitDriver();
    }
}
