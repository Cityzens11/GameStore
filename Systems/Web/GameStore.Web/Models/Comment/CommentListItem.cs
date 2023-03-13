namespace GameStore.Web.Models;

public class CommentListItem
{
    public int Id { get; set; }
    public string User { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CommentedTime { get; set; }
    public int GameId { get; set; }
}
