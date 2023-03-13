using AutoMapper;
using GameStore.Common.Filters;
using GameStore.Common.Validator;
using GameStore.Context;
using GameStore.Context.Entities;
using GameStore.Services.Games;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace GameStore.Tests.Services;

public class GameServiceTests
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IDbContextFactory<MainDbContext>> contextFactoryMock;
    private readonly Mock<IModelValidator<AddGameModel>> addGameModelValidatorMock;
    private readonly Mock<IModelValidator<UpdateGameModel>> updateGameModelValidatorMock;
    private readonly GameService gameService;

    public GameServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        contextFactoryMock = new Mock<IDbContextFactory<MainDbContext>>();
        addGameModelValidatorMock = new Mock<IModelValidator<AddGameModel>>();
        updateGameModelValidatorMock = new Mock<IModelValidator<UpdateGameModel>>();

        gameService = new GameService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addGameModelValidatorMock.Object,
            updateGameModelValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetGames_ShouldReturnListOfGames()
    {
        // Arrange
        var games = new List<Game>
        {
            new Game { Id = 1, Title = "Game 1" },
            new Game { Id = 2, Title = "Game 2" }
        };
        var gameModels = new List<GameModel>
        {
            new GameModel { Id = 1, Name = "Game 1" },
            new GameModel { Id = 2, Name = "Game 2" }
        } as IEnumerable<GameModel>;
        var queryableGames = games.AsQueryable(); ;
        var offset = 0;
        var limit = 20;

        var contextMock = new Mock<MainDbContext>();
        contextMock.Setup(x => x.Games).ReturnsDbSet(queryableGames);
        contextFactoryMock.Setup(x => x.CreateDbContextAsync(default(CancellationToken))).ReturnsAsync(contextMock.Object);

        mapperMock.Setup(x => x.Map<GameModel>(It.IsAny<Game>())).Returns(
                (Game game) => new GameModel
                {
                    Name = game.Title,
                    Id = game.Id,
                }
            );

        // Act
        var result = await gameService.GetGames(offset, limit);

        var result1 = result.FirstOrDefault();
        var result2 = result.Last();
        var expected1 = gameModels.FirstOrDefault();
        var expected2 = gameModels.Last();

        // Assert
        Assert.Equal(result1.Name, expected1.Name);
        Assert.Equal(result1.Id, expected1.Id);

        Assert.Equal(result2.Name, expected2.Name);
        Assert.Equal(result2.Id, expected2.Id);
    }

    [Fact]
    public async Task GetGame_WhenGameExists_ShouldReturnGameModel()
    {
        // Arrange
        var gameId = 1;
        var game = new Game { Id = gameId, Title = "Game 1" };
        var gameModel = new GameModel { Id = gameId, Name = "Game 1" };

        var contextMock = new Mock<MainDbContext>();
        contextMock.Setup(x => x.Games).ReturnsDbSet(new[] { game });
        contextFactoryMock.Setup(x => x.CreateDbContextAsync(default(CancellationToken))).ReturnsAsync(contextMock.Object);

        mapperMock.Setup(x => x.Map<GameModel>(game)).Returns(gameModel);

        // Act
        var result = await gameService.GetGame(gameId);

        // Assert
        Assert.Equal(result, gameModel);
    }

    [Fact]
    public async Task GetGame_WhenGameDoesNotExist_ShouldThrowNullReferenceException()
    {
        // Arrange
        var gameId = 1;

        var contextMock = new Mock<MainDbContext>();
        contextMock.Setup(x => x.Games).ReturnsDbSet(new List<Game>());
        contextFactoryMock.Setup(x => x.CreateDbContextAsync(default(CancellationToken))).ReturnsAsync(contextMock.Object);

        // Act
        var result = await gameService.GetGame(gameId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddGame_ShouldReturnGameModel()
    {
        // Arrange
        var addGame = new AddGameModel { Name = "Test", Note = "TestDescription", Price = 3.5f, Publisher = "TestPublisher" };
        var game = new Game { Title = "Test", Description = "TestDescription", Price = 3.5f, Publisher = "TestPublisher" };
        var expectedResult = new GameModel { Name = "Test", Note = "TestDescription", Price = 3.5f, Publisher = "TestPublisher" };

        var contextMock = new Mock<MainDbContext>();
        contextMock.Setup(x => x.Games).ReturnsDbSet(new[] { game });
        contextFactoryMock.Setup(x => x.CreateDbContextAsync(default(CancellationToken))).ReturnsAsync(contextMock.Object);

        mapperMock.Setup(x => x.Map<Game>(addGame)).Returns(game);
        mapperMock.Setup(x => x.Map<GameModel>(game)).Returns(expectedResult);

        // Act
        var actualResult = await gameService.AddGame(addGame);

        // Assert
        Assert.NotNull(actualResult);
        Assert.IsType<GameModel>(actualResult);
        Assert.Equal(addGame.Name, actualResult.Name);
        Assert.Equal(addGame.Note, actualResult.Note);
        Assert.Equal(addGame.Price, actualResult.Price);
        Assert.Equal(addGame.Publisher, actualResult.Publisher);
    }
}
