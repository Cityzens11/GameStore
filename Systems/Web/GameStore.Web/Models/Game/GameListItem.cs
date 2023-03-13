namespace GameStore.Web.Models;

public class GameListItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public string ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string> Genres { get; set; }
}
