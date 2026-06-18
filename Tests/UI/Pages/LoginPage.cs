using OpenQA.Selenium;
using Tests.Core.Pages;

namespace Tests.UI.Pages;

public class LoginPage : BasePage
{
    private static readonly By UsernameInput = By.Id("userName");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By LoginButton = By.Id("login");
    private static readonly By ErrorMessage = By.Id("name");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Open() => NavigateTo("https://demoqa.com/login");

    public void EnterUsername(string username) => WaitForElement(UsernameInput).SendKeys(username);

    public void EnterPassword(string password) => WaitForElement(PasswordInput).SendKeys(password);

    public void ClickLogin() => WaitForClickable(LoginButton).Click();

    public void Login(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        ClickLogin();
    }

    public string GetErrorMessage() => WaitForElement(ErrorMessage).Text;
}
