namespace GameStore.Identity.Configuration;

using Duende.IdentityServer.Services;

/// <summary>
/// CORS configuration
/// </summary>
public static class CorsConfiguration
{
    /// <summary>
    /// Add CORS
    /// </summary>
    /// <param name="services">Services</param>
    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services.AddSingleton<ICorsPolicyService>((container) =>
        {
            var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

            return new DefaultCorsPolicyService(logger)
            {
                AllowAll = true
            };
        });

        return services;
    }
}
