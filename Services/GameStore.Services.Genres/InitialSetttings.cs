using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Genres;

public static class InitialSettings
{
    public static IServiceCollection AddGenreService(this IServiceCollection services)
    {
        services.AddSingleton<IGenreService, GenreService>();

        return services;
    }
}
