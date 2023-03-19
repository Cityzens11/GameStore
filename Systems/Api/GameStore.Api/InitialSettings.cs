using GameStore.Api.Settings;
using GameStore.Services.Settings;
using GameStore.Services.UserAccount;
using GameStore.Services.Games;
using GameStore.Services.Comments;
using GameStore.Services.Carts;

namespace GameStore.Api;

public static class InitialSettings
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddIdentitySettings()
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddApiSpecialSettings()
            .AddGameService()
            .AddCommentService()
            .AddUserAccountService()
            .AddCartService()
            ;

        return services;
    }
}
