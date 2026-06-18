using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Tests.Core.Helpers;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Tests.Core.Drivers;

public static class DriverFactory
{
    public static IWebDriver CreateDriver()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), WebDriverManager.Helpers.VersionResolveStrategy.MatchingBrowser);

        var options = new ChromeOptions();
        if (ConfigReader.HeadlessBrowser)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        return new ChromeDriver(options);
    }
}
