using OpenQA.Selenium;

namespace Tests.Core.Drivers;

public static class DriverManager
{
    private static readonly ThreadLocal<IWebDriver> _driver = new();

    public static IWebDriver Driver => _driver.Value
        ?? throw new InvalidOperationException("WebDriver has not been initialized for this thread.");

    public static void SetDriver(IWebDriver driver) => _driver.Value = driver;

    public static void QuitDriver()
    {
        _driver.Value?.Quit();
        _driver.Value = null!;
    }
}
