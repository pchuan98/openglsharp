using Serilog;

namespace OpenGlSharp.Extensions.Serilog;

/// <summary>
/// 快捷使用的extension
/// </summary>
public static class SerilogBaseExtension
{
    public static void ConfigLoggerDefault(this ILogger _) => ConfigLoggerDefault();

    public static void ConfigLoggerDefault()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithCallerInfo(["OpenGlSharp", "OpenGlSharp.Study"])
            .MinimumLevel.Verbose()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:}] "
                                + "[{Namespace} | {Method}] "
                                + "{Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();
    }
}