using Serilog;
using Serilog.Events;

namespace Tests.Core.Helpers;

public static class Log
{
    private static ILogger? _logger;

    public static ILogger Logger => _logger ??= CreateLogger();

    public static void Initialize() => _logger = CreateLogger();

    private static ILogger CreateLogger()
    {
        var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        Directory.CreateDirectory(logDir);

        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: Path.Combine(logDir, "test-run-.log"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    public static void Info(string message) => Logger.Information(message);
    public static void Debug(string message) => Logger.Debug(message);
    public static void Warning(string message) => Logger.Warning(message);
    public static void Error(string message, Exception? ex = null) => Logger.Error(ex, message);
}
