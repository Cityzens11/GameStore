using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Comments;

public static class InitialSettings
{
    public static IServiceCollection AddCommentService(this IServiceCollection services)
    {
        services.AddSingleton<ICommentService, CommentService>();

        return services;
    }
}
