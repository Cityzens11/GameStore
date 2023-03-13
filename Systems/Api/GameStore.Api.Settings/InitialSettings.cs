namespace GameStore.Api.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameStore.Settings;

public static class InitialSettings
{
    public static IServiceCollection AddApiSpecialSettings(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<SpecialApiSettings>("ApiSpecial", configuration);
        services.AddSingleton(settings);

        return services;
    }
}
