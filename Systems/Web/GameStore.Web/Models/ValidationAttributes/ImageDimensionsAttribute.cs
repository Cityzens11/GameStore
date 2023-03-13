namespace GameStore.Web.Models;

using System.ComponentModel.DataAnnotations;
using System.Drawing;


public class ImageDimensionsAttribute : ValidationAttribute
{
    private readonly int _width;
    private readonly int _height;

    public ImageDimensionsAttribute(int width, int height)
    {
        _width = width;
        _height = height;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var file = value as IFormFile;
        if (file == null)
        {
            return ValidationResult.Success;
        }

        using (var image = Image.FromStream(file.OpenReadStream()))
        {
            if (image.Width != _width || image.Height != _height)
            {
                return new ValidationResult($"The image must be {_height}x{_width} pixels.");
            }
        }

        return ValidationResult.Success;
    }
}

