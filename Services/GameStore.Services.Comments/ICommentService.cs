namespace GameStore.Services.Comments;

public interface ICommentService
{
    Task<IEnumerable<CommentModel>> GetComments(int gameId);
    Task<CommentModel> GetComment(int commentId);
    Task<CommentModel> AddComment(AddCommentModel model);
    Task UpdateComment(int commentId, UpdateCommentModel model);
    Task DeleteComment(int commentId);
}
