namespace GameStore.Context;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameStore.Settings;

public static class InitialSettings
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Settings.Load<DbSettings>("Database", configuration);
        services.AddSingleton(settings);

        var dbInitOptionsDelegate = DbContextOptionsFactory.Configure(settings.ConnectionString);

        services.AddDbContextFactory<MainDbContext>(dbInitOptionsDelegate);


        return services;
    }
}
