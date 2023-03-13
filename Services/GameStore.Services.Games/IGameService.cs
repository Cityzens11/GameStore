using GameStore.Common.Filters;

namespace GameStore.Services.Games;

public interface IGameService
{
    Task<IEnumerable<GameModel>> GetGames(int offset = 0, int limit = 12);
    Task<IEnumerable<GameModel>> GetGames(Filter filter, int offset = 0, int limit = 12);
    Task<GameModel> GetGame(int gameId);
    Task<GameModel> AddGame(AddGameModel model);
    Task UpdateGame(int gameId, UpdateGameModel model);
    Task DeleteGame(int gameId);
    Task<int> GetGamesCount();
    Task<int> GetGamesCount(Filter filter);
}
