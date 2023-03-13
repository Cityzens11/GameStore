using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Common.Helpers;

public static class AutoMapperRegisterHelper
{
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.ToUpperInvariant().StartsWith("GAMESTORE."));

        services.AddAutoMapper(assemblies);
    }
}

