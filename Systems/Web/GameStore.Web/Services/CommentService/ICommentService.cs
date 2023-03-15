namespace GameStore.Web.Services;

using System.Threading.Tasks;
using GameStore.Web.Models;

public interface ICommentService
{
    Task<IEnumerable<CommentListItem>> GetCommentsAsync(int gameId);
    Task<CommentListItem> GetCommentAsync(int commentId);
    Task AddCommentAsync(CommentModel model);
    Task EditCommentAsync(CommentModel model);
    Task DeleteCommentAsync(int commentId);
}