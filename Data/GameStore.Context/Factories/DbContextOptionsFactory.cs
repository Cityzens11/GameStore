namespace GameStore.Context;

using Microsoft.EntityFrameworkCore;
using System.Data;

public static class DbContextOptionsFactory
{
    private const string migrationPath = "GameStore.Context.Migrations";

    public static DbContextOptions<MainDbContext> Create(string connStr)
    {
        var bldr = new DbContextOptionsBuilder<MainDbContext>();

        Configure(connStr).Invoke(bldr);

        return bldr.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connStr)
    {
        return (bldr) =>
        {
            bldr.UseSqlServer(connStr,
                        opts => opts
                                .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                                .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                                .MigrationsAssembly($"{migrationPath}")
                        );

            bldr.EnableSensitiveDataLogging();
            //bldr.UseLazyLoadingProxies();
            bldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        };
    }
}
