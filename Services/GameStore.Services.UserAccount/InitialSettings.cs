using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.UserAccount;

public static class InitialSettings
{
    public static IServiceCollection AddUserAccountService(this IServiceCollection services)
    {
        services.AddScoped<IUserAccountService, UserAccountService>();

        return services;
    }
}
