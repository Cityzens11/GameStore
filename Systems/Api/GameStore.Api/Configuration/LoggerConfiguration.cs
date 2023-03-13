using Serilog;

namespace GameStore.Api.Configuration;

/// <summary>
/// Logger Configuration
/// </summary>
public static class LoggerConfiguration
{
    /// <summary>
    /// Add logger to builder
    /// </summary>
    /// <param name="builder"></param>
    public static void AddAppLogger(this WebApplicationBuilder builder)
    {
        var logger = new Serilog.LoggerConfiguration()
            .Enrich.WithCorrelationIdHeader()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog(logger, true);
    }
}
