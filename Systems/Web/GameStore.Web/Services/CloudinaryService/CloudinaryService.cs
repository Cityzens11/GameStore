namespace GameStore.Web.Services;

using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using GameStore.Web.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinarySettings _cloudinarySettings;

    public CloudinaryService(CloudinarySettings cloudinarySettings)
    {
        _cloudinarySettings = cloudinarySettings;
    }

    public async Task<string> UploadCloudinary(IFormFile image, string forlderName)
    {
        var api_name = _cloudinarySettings.CloudName;
        var api_key = _cloudinarySettings.ApiKey;
        var api_secret = _cloudinarySettings.ApiSecret;

        Account account = new Account(api_name, api_key, api_secret);
        Cloudinary cloudinary = new Cloudinary(account);
        cloudinary.Api.Secure = true;

        using var stream = new MemoryStream();
        await image.CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(image.FileName, stream),
            PublicId = forlderName + "/" + image.FileName
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.AbsoluteUri;
    }
}
