using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace GameStore.Api.Configuration.HealthChecks;

public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("GameStore.Api");
        var versionNumber = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
    }
}
