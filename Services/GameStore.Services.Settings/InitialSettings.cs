namespace GameStore.Services.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameStore.Settings;

public static class InitialSettings
{
    public static IServiceCollection AddMainSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Settings.Load<MainSettings>("Main", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Settings.Load<SwaggerSettings>("Swagger", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddIdentitySettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Settings.Load<IdentitySettings>("Identity", configuration);
        services.AddSingleton(settings);

        return services;
    }
}
