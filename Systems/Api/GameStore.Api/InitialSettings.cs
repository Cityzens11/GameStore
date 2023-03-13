using GameStore.Api.Settings;
using GameStore.Services.Settings;
using GameStore.Services.UserAccount;
using GameStore.Services.Games;
using GameStore.Services.Comments;

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
            ;

        return services;
    }
}
