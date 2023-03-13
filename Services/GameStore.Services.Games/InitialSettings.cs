using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Games;

public static class InitialSettings
{
    public static IServiceCollection AddGameService(this IServiceCollection services)
    {
        services.AddSingleton<IGameService, GameService>();

        return services;
    }
}
