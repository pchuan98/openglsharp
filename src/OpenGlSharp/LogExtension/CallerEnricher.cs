using Serilog.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Immutable;
using System.Diagnostics;

namespace OpenGlSharp.LogExtension;

public class CallerEnricher(
    IEnumerable<string> allowedAssemblies)
    : ILogEventEnricher
{
    private readonly ImmutableHashSet<string> _allowedAssemblies
        = allowedAssemblies.ToImmutableHashSet(equalityComparer: StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Add information about the origin of the logged message, such as method, namespace, and file information (from debugging symbols).
    /// </summary>
    /// <param name="logEvent">The logged event.</param>
    /// <param name="propertyFactory">The property factory</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var st = (new StackTrace()).GetFrames();

        var names = st.Select(item => item.GetMethod()?.Name).ToList();

        var frame = st.FirstOrDefault(stack =>
        {
            if (!stack.HasMethod()) return false;

            var type = stack.GetMethod()?.DeclaringType;
            if (type is null) return false;

            if (stack.GetMethod()?.Name == "Enrich"
                || type.FullName == "CallerEnricher") return false;

            var name = type.Assembly.GetName().Name;

            return name is not null && _allowedAssemblies.Contains(name);
        });

        var method = frame?.GetMethod();
        var type = method?.DeclaringType;

        var demo = frame.GetFileName();

        var methodName = method?.Name ?? "Unknown";
        var methodNamespace = type?.FullName ?? "Unknown";

        logEvent.AddPropertyIfAbsent(new LogEventProperty($"Method", new ScalarValue(methodName)));
        logEvent.AddPropertyIfAbsent(new LogEventProperty($"Namespace", new ScalarValue(methodNamespace)));
    }
}
public static class EnricherConfiguration
{
    /// <summary>
    /// Enrich log events with information about the calling method.
    /// </summary>
    /// <param name="enrichmentConfiguration">The enrichment configuration.</param>
    /// <param name="allowedAssemblies">Which assemblies to consider when finding the calling method in the stack trace.</param>
    /// <returns>The modified logger configuration.</returns>
    public static LoggerConfiguration WithCallerInfo(
        this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IEnumerable<string> allowedAssemblies) => enrichmentConfiguration.With(new CallerEnricher(allowedAssemblies));
}