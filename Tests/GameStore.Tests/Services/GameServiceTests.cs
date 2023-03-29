using AutoMapper;
using GameStore.Common.Filters;
using GameStore.Common.Validator;
using GameStore.Context;
using GameStore.Context.Entities;
using GameStore.Services.Games;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace GameStore.Tests.Services;

public class GameServiceTests : IDisposable
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IDbContextFactory<MainDbContext>> contextFactoryMock;
    private readonly Mock<IModelValidator<AddGameModel>> addGameModelValidatorMock;
    private readonly Mock<IModelValidator<UpdateGameModel>> updateGameModelValidatorMock;
    private readonly DbContextOptions<MainDbContext> _options;

    public GameServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        addGameModelValidatorMock = new Mock<IModelValidator<AddGameModel>>();
        updateGameModelValidatorMock = new Mock<IModelValidator<UpdateGameModel>>();
        contextFactoryMock = new Mock<IDbContextFactory<MainDbContext>>();

        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public async Task GetGames_ShouldReturnAllGames()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var games = new List<Game>
        {
            new Game { Id = 1, Title = "Game 1", Description = "Description 1", ImageUri = "Image 1",  Publisher = "Publisher 1"},
            new Game { Id = 2, Title = "Game 2", Description = "Description 2", ImageUri = "Image 2",  Publisher = "Publisher 2"},
            new Game { Id = 3, Title = "Game 3", Description = "Description 3", ImageUri = "Image 3",  Publisher = "Publisher 3"}
        };

        await context.Games.AddRangeAsync(games);
        await context.SaveChangesAsync();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<GameModel>(It.IsAny<Game>())).Returns(
                (Game game) => new GameModel
                {
                    Name = game.Title,
                    Id = game.Id,
                }
            );

        var gameService = new GameService(
            mapperMock.Object, 
            contextFactoryMock.Object, 
            addGameModelValidatorMock.Object, 
            updateGameModelValidatorMock.Object
            );

        // Act
        var result = await gameService.GetGames();

        // Assert
        Assert.Equal(games.Count, result.Count());
        Assert.Equal(games.Select(g => g.Id), result.Select(g => g.Id));
    }

    [Fact]
    public async Task GetGames_WithFilter_ShouldReturnFilteredGames()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var games = new List<Game>
        {
            new Game { Id = 1, Title = "Game 1", Description = "Description 1", ImageUri = "Image 1",  Publisher = "Publisher 1", Genres = new List<Genre> { new Genre { Name = "Action" }, new Genre { Name = "Adventure" } } },
            new Game { Id = 2, Title = "Game 2", Description = "Description 2", ImageUri = "Image 2",  Publisher = "Publisher 2", Genres = new List<Genre> { new Genre { Name = "Simulation" }, new Genre { Name = "Survival" } } },
            new Game { Id = 3, Title = "Game 3", Description = "Description 3", ImageUri = "Image 3",  Publisher = "Publisher 3", Genres = new List<Genre> { new Genre { Name = "Action" }, new Genre { Name = "Strategy" } } }
        };

        await context.Games.AddRangeAsync(games);
        await context.SaveChangesAsync();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<GameModel>(It.IsAny<Game>())).Returns(
                (Game game) => new GameModel
                {
                    Name = game.Title,
                    Id = game.Id,
                }
            );

        var gameService = new GameService(
            mapperMock.Object, 
            contextFactoryMock.Object, 
            addGameModelValidatorMock.Object, 
            updateGameModelValidatorMock.Object
            );

        var filter = new Filter { Genres = new List<string> { "Action", "Adventure" } };

        // Act
        var result = await gameService.GetGames(filter);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(new List<int> { 1, 3 }, result.Select(g => g.Id).ToList());
    }

    [Fact]
    public async Task GetGame_ShouldReturnGame()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var game = new Game { Id = 1, Title = "Game 1", Description = "Description 3", ImageUri = "Image 3", Publisher = "Publisher 3" };

        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<GameModel>(It.IsAny<Game>())).Returns(
                (Game game) => new GameModel
                {
                    Name = game.Title,
                    Id = game.Id,
                }
            );

        var gameService = new GameService(
            mapperMock.Object, 
            contextFactoryMock.Object, 
            addGameModelValidatorMock.Object, 
            updateGameModelValidatorMock.Object
            );

        // Act
        var result = await gameService.GetGame(1);

        // Assert
        Assert.Equal(game.Id, result.Id);
        Assert.Equal(game.Title, result.Name);
    }

    [Fact]
    public async Task AddGame_ShouldAddGameToDatabase()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var model = new AddGameModel
        {
            Name = "Test Game",
            Note = "Test Description",
            Publisher = "Test Publisher",
            ImageUri = "Test Image"
        };

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);
        mapperMock.Setup(x => x.Map<GameModel>(It.IsAny<Game>())).Returns(
                (Game game) => new GameModel
                {
                    Name = game.Title,
                    Id = game.Id,
                }
            );
        mapperMock.Setup(x => x.Map<Game>(It.IsAny<AddGameModel>())).Returns(
            (AddGameModel model) => new Game
            {
                Description = model.Note,
                ImageUri = model.ImageUri,
                Publisher = model.Publisher,
                Title = model.Name
            });

        var gameService = new GameService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addGameModelValidatorMock.Object,
            updateGameModelValidatorMock.Object
            );

        // Act
        var game = await gameService.AddGame(model);

        // Assert
        using var cn = new MainDbContext(_options);
        var savedGame = await cn.Games.FirstOrDefaultAsync(x => x.Id == game.Id);
        Assert.NotNull(savedGame);
        Assert.Equal(game.Name, savedGame.Title);
    }

    [Fact]
    public async Task DeleteGame_ShouldDeleteGameFromDatabase()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var game = new Game
        {
            Title = "Test Game",
            Description = "Test Description",
            Publisher = "Test Publiser",
            ImageUri = "Test Image"
        };

        context.Games.Add(game);
        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var gameService = new GameService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addGameModelValidatorMock.Object,
            updateGameModelValidatorMock.Object
            );

        // Act
        await gameService.DeleteGame(game.Id);

        // Assert
        using var cn = new MainDbContext(_options);
        var savedGame = await cn.Games.FirstOrDefaultAsync(x => x.Id == game.Id);
        Assert.Null(savedGame);
    }

    [Fact]
    public async Task GetGamesCount_ReturnsTotalCount()
    {
        // Arrange
        using var context = new MainDbContext(_options);
        var genres = new List<Genre>
        {
            new Genre { Name = "Action" },
            new Genre { Name = "Adventure" },
            new Genre { Name = "RPG" }
        };

        var games = new List<Game>
        {
            new Game { Title = "Game 1", Description = "Description 1", ImageUri = "Image 1", Publisher = "Publisher 1", Genres = new List<Genre> { genres[0], genres[1] } },
            new Game { Title = "Game 2", Description = "Description 2", ImageUri = "Image 2", Publisher = "Publisher 2", Genres = new List<Genre> { genres[0], genres[2] } },
            new Game { Title = "Game 3", Description = "Description 3", ImageUri = "Image 3", Publisher = "Publisher 3", Genres = new List<Genre> { genres[1] } }
        };

        context.Genres.AddRange(genres);
        context.Games.AddRange(games);

        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var gameService = new GameService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addGameModelValidatorMock.Object,
            updateGameModelValidatorMock.Object
            );

        // Act
        var result = await gameService.GetGamesCount();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task GetGamesCount_WithFilter_ReturnsFilteredCount()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var genres = new List<Genre>
        {
            new Genre { Name = "Action" },
            new Genre { Name = "Adventure" },
            new Genre { Name = "RPG" }
        };

        var games = new List<Game>
        {
            new Game { Title = "Game 1", Description = "Description 1", ImageUri = "Image 1", Publisher = "Publisher 1", Genres = new List<Genre> { genres[0], genres[1] } },
            new Game { Title = "Game 2", Description = "Description 2", ImageUri = "Image 2", Publisher = "Publisher 2", Genres = new List<Genre> { genres[0], genres[2] } },
            new Game { Title = "Game 3", Description = "Description 3", ImageUri = "Image 3", Publisher = "Publisher 3", Genres = new List<Genre> { genres[1] } }
        };

        context.Genres.AddRange(genres);
        context.Games.AddRange(games);

        context.SaveChanges();

        var filter = new Filter { Genres = new List<string> { "Action" } };

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var gameService = new GameService(
            mapperMock.Object,
            contextFactoryMock.Object,
            addGameModelValidatorMock.Object,
            updateGameModelValidatorMock.Object
            );

        // Act
        var result = await gameService.GetGamesCount(filter);

        // Assert
        Assert.Equal(2, result);
    }


    public void Dispose()
    {
        var context = new MainDbContext(_options);
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
