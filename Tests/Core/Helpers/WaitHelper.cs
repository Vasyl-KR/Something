using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tests.Core.Helpers;

public static class WaitHelper
{
    public static IWebElement WaitForElement(IWebDriver driver, By locator, int? timeoutSeconds = null)
    {
        var timeout = TimeSpan.FromSeconds(timeoutSeconds ?? ConfigReader.DefaultTimeoutSeconds);
        var wait = new WebDriverWait(driver, timeout);
        return wait.Until(d => d.FindElement(locator));
    }

    public static bool WaitForElementToDisappear(IWebDriver driver, By locator, int? timeoutSeconds = null)
    {
        var timeout = TimeSpan.FromSeconds(timeoutSeconds ?? ConfigReader.DefaultTimeoutSeconds);
        var wait = new WebDriverWait(driver, timeout);
        return wait.Until(d =>
        {
            try { return !d.FindElement(locator).Displayed; }
            catch (NoSuchElementException) { return true; }
        });
    }

    public static IWebElement WaitForElementToBeClickable(IWebDriver driver, By locator, int? timeoutSeconds = null)
    {
        var timeout = TimeSpan.FromSeconds(timeoutSeconds ?? ConfigReader.DefaultTimeoutSeconds);
        var wait = new WebDriverWait(driver, timeout);
        return wait.Until(d =>
        {
            var el = d.FindElement(locator);
            return el.Enabled && el.Displayed ? el : null;
        })!;
    }
}
