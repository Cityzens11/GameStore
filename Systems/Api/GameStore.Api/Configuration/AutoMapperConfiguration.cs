using GameStore.Common.Helpers;

namespace GameStore.Api.Configuration;

/// <summary>
/// AutoMapper configuration
/// </summary>
public static class AutoMapperConfiguration
{
    /// <summary>
    /// Add automappers
    /// </summary>
    /// <param name="services">Services collection</param>
    public static IServiceCollection AddAppAutoMappers(this IServiceCollection services)
    {
        AutoMapperRegisterHelper.Register(services);

        return services;
    }
}
