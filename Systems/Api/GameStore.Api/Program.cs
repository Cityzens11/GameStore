namespace GameStore.Api;

using GameStore.Api.Configuration;
using GameStore.Context;
using GameStore.Services.Settings;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var identitySettings = GameStore.Settings.Settings.Load<IdentitySettings>("Identity");
        var swaggerSettings = GameStore.Settings.Settings.Load<SwaggerSettings>("Swagger");

        var services = builder.Services;
        
        // Add services to the container.

        builder.AddAppLogger();

        services.AddHttpContextAccessor();

        services.AddAppCors();

        services.AddAppDbContext();

        services.AddAppAuth(identitySettings);

        services.AddAppHealthChecks();

        services.AddAppVersioning();

        services.AddAppSwagger(identitySettings, swaggerSettings);

        services.AddAppAutoMappers();

        services.AddAppControllerAndViews();

        services.RegisterAppServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAppCors();

        app.UseAppHealthChecks();

        app.UseAppSwagger();

        app.UseAppAuth();

        app.UseAppControllerAndViews();

        app.UseAppMiddlewares();

        //DbSeeder.Execute(app.Services, true, true);

        app.Run();
    }
}
