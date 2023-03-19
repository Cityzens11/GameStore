using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Carts;

public static class InitialSettings
{
    public static IServiceCollection AddCartService(this IServiceCollection services)
    {
        services.AddSingleton<ICartService, CartService>();

        return services;
    }
}
