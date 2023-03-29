using AutoMapper;
using GameStore.Common.Validator;
using GameStore.Context.Entities;
using GameStore.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using GameStore.Services.Comments;

namespace GameStore.Tests.Services;

public class CommentServiceTests
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IDbContextFactory<MainDbContext>> contextFactoryMock;
    private readonly Mock<IModelValidator<AddCommentModel>> addCommentModelValidatorMock;
    private readonly Mock<IModelValidator<UpdateCommentModel>> updateCommentModelValidatorMock;
    private readonly DbContextOptions<MainDbContext> _options;

    public CommentServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        addCommentModelValidatorMock = new Mock<IModelValidator<AddCommentModel>>();
        updateCommentModelValidatorMock = new Mock<IModelValidator<UpdateCommentModel>>();
        contextFactoryMock = new Mock<IDbContextFactory<MainDbContext>>();

        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public async Task GetComments_ShouldReturnAllCommentsForGame()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        context.Comments.Add(new Comment { Id = 10, GameId = 1, Author = "Author 1", Body = "Body 1", TimeStamp = DateTime.Now });
        context.Comments.Add(new Comment { Id = 20, GameId = 1, Author = "Author 2", Body = "Body 2", TimeStamp = DateTime.Now });
        context.Comments.Add(new Comment { Id = 30, GameId = 2, Author = "Author 3", Body = "Body 3", TimeStamp = DateTime.Now });
        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<CommentModel>(It.IsAny<Comment>())).Returns(
            (Comment comment) => new CommentModel
            {
                Id = comment.Id,
                GameId = comment.GameId,
                User = comment.Author,
                Body = comment.Body,
                TimeStamp = comment.TimeStamp
            }
         );

        var commentService = new CommentService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addCommentModelValidatorMock.Object,
            updateCommentModelValidatorMock.Object
            );

        // Act
        var result = await commentService.GetComments(1);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Id == 10);
        Assert.Contains(result, c => c.Id == 20);
    }

    [Fact]
    public async Task GetComment_ShouldReturnCommentById()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        context.Comments.Add(new Comment { Id = 4, GameId = 4, Author = "Author 1", Body = "Body 1", TimeStamp = DateTime.Now });
        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<CommentModel>(It.IsAny<Comment>())).Returns(
            (Comment comment) => new CommentModel
            {
                Id = comment.Id,
                GameId = comment.GameId,
                User = comment.Author,
                Body = comment.Body,
                TimeStamp = comment.TimeStamp
            }
         );

        var commentService = new CommentService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addCommentModelValidatorMock.Object,
            updateCommentModelValidatorMock.Object
            );

        // Act
        var result = await commentService.GetComment(4);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Id);
    }

    [Fact]
    public async Task AddComment_ShouldAddCommentToDatabase()
    {
        // Arrange
        using var context = new MainDbContext(_options);
        

        var model = new AddCommentModel();

        contextFactoryMock
           .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
           .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<CommentModel>(It.IsAny<Comment>())).Returns(
            (Comment comment) => new CommentModel
            {
                Id = comment.Id,
                GameId = comment.GameId,
                User = comment.Author,
                Body = comment.Body,
                TimeStamp = comment.TimeStamp
            }
         );
        mapperMock.Setup(x => x.Map<Comment>(It.IsAny<AddCommentModel>())).Returns(
            (AddCommentModel model) => new Comment
            {
                Author = model.User,
                Body = model.Body,
                TimeStamp= model.TimeStamp,
                GameId= model.GameId,
            });

        var commentService = new CommentService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addCommentModelValidatorMock.Object,
            updateCommentModelValidatorMock.Object
            );

        // Act
        var result = await commentService.AddComment(model);

        // Assert
        using var cn = new MainDbContext(_options);
        Assert.NotNull(result);
        Assert.Equal(1, cn.Comments.Count());
        Assert.Equal(1, result.Id);
        mapperMock.Verify(m => m.Map<Comment>(model), Times.Once);
    }

    //[Fact]
    //public async Task DeleteComment_ExistingCommentWithNoChildComments_ShouldDeleteComment()
    //{
    //    // Arrange
    //    using var context = new MainDbContext(_options);

    //    var comment = new Comment
    //    {
    //        Id = 10,
    //        GameId = 10,
    //        Author = "Test User",
    //        Body = "Test Body",
    //        TimeStamp = DateTime.Now,
    //    };

    //    context.Comments.Add(comment);
    //    context.SaveChanges();

    //    contextFactoryMock
    //        .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
    //        .ReturnsAsync(context);

    //    var commentService = new CommentService(
    //        mapperMock.Object,
    //        contextFactoryMock.Object,
    //        addCommentModelValidatorMock.Object,
    //        updateCommentModelValidatorMock.Object
    //        );

    //    // Act
    //    await commentService.DeleteComment(comment.Id);

    //    // Assert
    //    using var cn = new MainDbContext(_options);
    //    var savedComment = await cn.Comments.FirstOrDefaultAsync(x => x.Id == comment.Id);
    //    Assert.Null(savedComment);
    //}

    public void Dispose()
    {
        var context = new MainDbContext(_options);
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
