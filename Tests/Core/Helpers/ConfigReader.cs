using Microsoft.Extensions.Configuration;

namespace Tests.Core.Helpers;

public static class ConfigReader
{
    private static readonly IConfiguration _config = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: false)
        .AddEnvironmentVariables()
        .Build();

    public static string BaseUiUrl => _config["BaseUiUrl"]!;
    public static string BaseApiUrl => _config["BaseApiUrl"]!;
    public static bool HeadlessBrowser => bool.Parse(_config["HeadlessBrowser"] ?? "false");
    public static int DefaultTimeoutSeconds => int.Parse(_config["DefaultTimeoutSeconds"] ?? "10");
}
