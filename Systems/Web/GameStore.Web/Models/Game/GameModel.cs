namespace GameStore.Web.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class GameModel
{
    public int? Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;

    [ImageDimensions(400, 400, ErrorMessage = "Image must be 400x400 pixels")]
    public IFormFile? Image { get; set; }

    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }

    public IEnumerable<SelectListItem> GenreList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "Action", Text = "Action" },
        new SelectListItem { Value = "Adventure", Text = "Adventure" },
        new SelectListItem { Value = "Fantasy", Text = "Fantasy" },
        new SelectListItem { Value = "Shooter", Text = "Shooter" },
        new SelectListItem { Value = "RPG", Text = "RPG" },
        new SelectListItem { Value = "Survival", Text = "Survival" },
        new SelectListItem { Value = "Horror", Text = "Horror" },
        new SelectListItem { Value = "MOBA", Text = "MOBA" },
        new SelectListItem { Value = "Strategy", Text = "Strategy" },
        new SelectListItem { Value = "Stealth", Text = "Stealth" },
        new SelectListItem { Value = "Arcade", Text = "Arcade" },
        new SelectListItem { Value = "Sports", Text = "Sports" },
        new SelectListItem { Value = "Races", Text = "Races" },
        new SelectListItem { Value = "Puzzle", Text = "Puzzle" },
        new SelectListItem { Value = "Other", Text = "Other" }
    };
}

