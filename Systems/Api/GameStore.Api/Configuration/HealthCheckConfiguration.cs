using GameStore.Api.Configuration.HealthChecks;
using GameStore.Common.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


namespace GameStore.Api.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<SelfHealthCheck>("GameStore.Api");

        return services;
    }

    public static void UseAppHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        app.MapHealthChecks("/health/detail", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckHelper.WriteHealthCheckResponse,
            AllowCachingResponses = false,
        });
    }
}
