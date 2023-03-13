using GameStore.Common.Extensions;

namespace GameStore.Api.Configuration;

public static class ControllerAndViewsConfiguration
{
    public static IServiceCollection AddAppControllerAndViews(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.SetDefaultSettings())
            .AddValidator()
            ;

        return services;
    }

    public static IEndpointRouteBuilder UseAppControllerAndViews(this IEndpointRouteBuilder app)
    {
        app.MapControllers();

        return app;
    }
}
