namespace GameStore.Identity.Configuration;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

/// <summary>
/// HealthCheck
/// </summary>
public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("GameStore.Identity");
        var versionNumber = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
    }
}
