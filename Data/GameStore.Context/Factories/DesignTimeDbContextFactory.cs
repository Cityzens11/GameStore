namespace GameStore.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using GameStore.Settings;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    private const string migrationPath = "GameStore.Context.Migrations";

    public MainDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

        var settings = Settings.Load<DbSettings>("Database", configuration);

        DbContextOptions<MainDbContext> options = new DbContextOptionsBuilder<MainDbContext>()
                    .UseSqlServer(
                        settings.ConnectionString,
                        opts => opts
                            .MigrationsAssembly($"{migrationPath}")
                    )
                    .Options;

        var dbf = new DbContextFactory(options);
        return dbf.Create();
    }
}
