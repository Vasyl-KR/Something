using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Tests.Core.Fixtures;
using Tests.UI.Pages;

namespace Tests.UI.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("UI")]
[AllureFeature("Login")]
public class LoginTests : UiTestFixture
{
    private LoginPage _loginPage = null!;

    [SetUp]
    public void SetUp()
    {
        _loginPage = new LoginPage(Driver);
        _loginPage.Open();
    }

    [Test]
    [AllureTag("Smoke")]
    [AllureName("Login page title is correct")]
    public void LoginPage_HasCorrectTitle()
    {
        Assert.That(_loginPage.PageTitle, Does.Contain("demosite"));
    }

    [Test]
    [AllureName("Login with invalid credentials shows error")]
    public void Login_WithInvalidCredentials_ShowsError()
    {
        _loginPage.Login("invalid_user", "invalid_pass");

        Assert.That(_loginPage.GetErrorMessage(), Is.Not.Empty);
    }
}
