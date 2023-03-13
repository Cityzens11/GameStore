namespace GameStore.Web.Services;

public interface ICloudinaryService
{
    Task<string> UploadCloudinary(IFormFile image, string forlderName);
}
