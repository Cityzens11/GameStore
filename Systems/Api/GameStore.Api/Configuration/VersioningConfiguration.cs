using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Configuration;

/// <summary>
/// Versioning configuration
/// </summary>
public static class VersioningConfiguration
{
    /// <summary>
    /// Add version support for API
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns></returns>
    public static IServiceCollection AddAppVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
