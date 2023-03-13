using GameStore.Context;
using GameStore.Identity.Configuration;

namespace GameStore.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        builder.AddAppLogger();

        services.AddAppCors();

        services.AddHttpContextAccessor();

        services.AddAppDbContext();

        services.AddAppHealthChecks();

        services.RegisterAppServices();

        services.AddIS4();

        var app = builder.Build();

        app.UseAppHealthChecks();

        app.UseIS4();

        app.Run();
    }
}
