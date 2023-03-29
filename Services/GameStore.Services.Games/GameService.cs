namespace GameStore.Services.Games;

using AutoMapper;
using GameStore.Common;
using GameStore.Common.Exceptions;
using GameStore.Common.Filters;
using GameStore.Common.Validator;
using GameStore.Context;
using GameStore.Context.Entities;
using Microsoft.EntityFrameworkCore;

public class GameService : IGameService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IModelValidator<AddGameModel> addGameModelValidator;
    private readonly IModelValidator<UpdateGameModel> updateGameModelValidator;

    public GameService(
        IMapper mapper,
        IDbContextFactory<MainDbContext> contextFactory,
        IModelValidator<AddGameModel> addGameModelValidator,
        IModelValidator<UpdateGameModel> updateGameModelValidator
        )
    {
        this.mapper = mapper;
        this.contextFactory = contextFactory;
        this.addGameModelValidator = addGameModelValidator;
        this.updateGameModelValidator = updateGameModelValidator;
    }

    public async Task<IEnumerable<GameModel>> GetGames(int offset = 0, int limit = 12)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var games = context.Games
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, Math.Min(limit, 1000)))
                .Include(x => x.Genres);

        var data = (await games.ToListAsync()).Select(game => mapper.Map<GameModel>(game));

        return data;
    }

    public async Task<IEnumerable<GameModel>> GetGames(Filter filter, int offset = 0, int limit = 12)
    {
        if (filter.Name is null && filter.Genres is null)
            throw new NullReferenceException();


        using var context = await contextFactory.CreateDbContextAsync();

        var games = context.Games.Include(x => x.Genres).AsQueryable();

        games = await Filter(games, filter);

        games = games
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data = (await games.ToListAsync()).Select(game => mapper.Map<GameModel>(game));

        return data;
    }

    public async Task<GameModel> GetGame(int gameId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var game = await context.Games.Include(x => x.Genres).FirstOrDefaultAsync(x => x.Id.Equals(gameId));

        var data = mapper.Map<GameModel>(game);

        return data;
    }
    public async Task<GameModel> AddGame(AddGameModel model)
    {
        addGameModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var game = mapper.Map<Game>(model);
        await context.Games.AddAsync(game);
        context.SaveChanges();

        return mapper.Map<GameModel>(game);
    }

    public async Task UpdateGame(int gameId, UpdateGameModel model)
    {
        updateGameModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var game = await context.Games.Include(x => x.Genres).FirstOrDefaultAsync(x => x.Id.Equals(gameId))
            ?? throw new ProcessException($"The game (id: {gameId}) was not found");

        if (model.Genres != null)
        {
            foreach (var genre in game.Genres)
            {
                context.Remove(genre);
            }
            context.SaveChanges();
        }

        game = mapper.Map(model, game);

        context.Games.Update(game);
        context.SaveChanges();
    }

    public async Task DeleteGame(int gameId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var game = await context.Games.FirstOrDefaultAsync(x => x.Id.Equals(gameId))
            ?? throw new ProcessException($"The game (id: {gameId}) was not found");

        var comments = context.Comments.Where(x => x.GameId == gameId);
        var cartItems = context.CartItems.Where(x => x.GameId == gameId);

        context.CartItems.RemoveRange(cartItems);
        context.Comments.RemoveRange(comments);
        context.Games.Remove(game);
        context.SaveChanges();
    }

    public async Task<int> GetGamesCount()
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var count = await context.Games.CountAsync();

        return count;
    }

    public async Task<int> GetGamesCount(Filter filter)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var games = context.Games.Include(x => x.Genres).AsQueryable();

        games = await Filter(games, filter);

        var count = await games.CountAsync();

        return count;
    }


    public async Task<IQueryable<Game>> Filter(IQueryable<Game> filtered, Filter filter)
    {
        var genres = filter.Genres;
        var name = filter.Name;

        if (genres != null)
        {
            filtered = filtered
                .Where(g => g.Genres.Any(genre => genres.Contains(genre.Name)));
        }

        if (!name.IsNullOrEmpty())
        {
            filtered = filtered.Where(g => g.Title.Contains(name));
        }

        return filtered;
    }
}
