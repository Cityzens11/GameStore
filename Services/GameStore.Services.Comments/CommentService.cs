using AutoMapper;
using GameStore.Common.Exceptions;
using GameStore.Common.Validator;
using GameStore.Context.Entities;
using GameStore.Context;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.Comments;

public class CommentService : ICommentService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IModelValidator<AddCommentModel> addCommentModelValidator;
    private readonly IModelValidator<UpdateCommentModel> updateCommentModelValidator;

    public CommentService(
        IMapper mapper,
        IDbContextFactory<MainDbContext> contextFactory,
        IModelValidator<AddCommentModel> addCommentModelValidator,
        IModelValidator<UpdateCommentModel> updateCommentModelValidator
        )
    {
        this.mapper = mapper;
        this.contextFactory = contextFactory;
        this.addCommentModelValidator = addCommentModelValidator;
        this.updateCommentModelValidator = updateCommentModelValidator;
    }

    public async Task<IEnumerable<CommentModel>> GetComments(int gameId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var comments = context.Comments.Where(c => c.GameId == gameId);

        var data = (await comments.ToListAsync()).Select(comment => mapper.Map<CommentModel>(comment));

        return data;
    }

    public async Task<CommentModel> GetComment(int commentId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(commentId));

        var data = mapper.Map<CommentModel>(comment);

        return data;
    }
    public async Task<CommentModel> AddComment(AddCommentModel model)
    {
        addCommentModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var comment = mapper.Map<Comment>(model);
        await context.Comments.AddAsync(comment);
        context.SaveChanges();

        return mapper.Map<CommentModel>(comment);
    }

    public async Task UpdateComment(int commentId, UpdateCommentModel model)
    {
        updateCommentModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(commentId))
            ?? throw new ProcessException($"The comment (id: {commentId}) was not found");

        comment = mapper.Map(model, comment);

        context.Comments.Update(comment);
        context.SaveChanges();
    }

    public async Task DeleteComment(int commentId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var comment = await context.Comments
            .Include(c => c.ChildComments)
            .FirstOrDefaultAsync(x => x.Id.Equals(commentId))
            ?? throw new ProcessException($"The comment (id: {commentId}) was not found");

        foreach (var childComment in comment.ChildComments.ToList())
        {
            await DeleteComment(childComment.Id);
        }
        comment.ChildComments = null;
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
    }
}
