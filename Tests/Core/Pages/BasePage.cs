using OpenQA.Selenium;
using Tests.Core.Helpers;

namespace Tests.Core.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
    }

    protected IWebElement WaitForElement(By locator, int? timeoutSeconds = null)
        => WaitHelper.WaitForElement(Driver, locator, timeoutSeconds);

    protected IWebElement WaitForClickable(By locator, int? timeoutSeconds = null)
        => WaitHelper.WaitForElementToBeClickable(Driver, locator, timeoutSeconds);

    protected void Click(By locator)
    {
        var element = WaitForClickable(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }

    protected void NavigateTo(string url) => Driver.Navigate().GoToUrl(url);

    public string PageTitle => Driver.Title;
    public string CurrentUrl => Driver.Url;
}
