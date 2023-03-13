namespace GameStore.Web.Cloudinary;

using GameStore.Settings;

public static class SetCloudinaryCredentials
{
    public static IServiceCollection AddCloudinarySettings(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<CloudinarySettings>("Cloudinary", configuration);
        services.AddSingleton(settings);

        return services;
    }
}
