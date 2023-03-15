namespace GameStore.Web.Models;

public class CommentHierarchy
{
    public int ParentId { get; set; }
    public IEnumerable<CommentListItem> Comments { get; set; }
}
