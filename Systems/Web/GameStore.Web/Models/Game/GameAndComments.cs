namespace GameStore.Web.Models;

public class GameAndComments
{
    public GameListItem Game { get; set; }
    public IEnumerable<CommentListItem> Comments { get; set; }

    public GameAndComments(GameListItem game, IEnumerable<CommentListItem> comments)
    {
        Game = game;
        Comments = comments;
    }
}
