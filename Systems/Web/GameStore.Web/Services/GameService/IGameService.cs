namespace GameStore.Web.Services;

using System.Threading.Tasks;
using GameStore.Common.Filters;
using GameStore.Web.Models;

public interface IGameService
{
    Task<IEnumerable<GameListItem>> GetGamesAsync(int offset = 0, int limit = 12, Filter filter = null);
    Task<GameListItem> GetGameAsync(int gameId);
    Task AddGameAsync(GameModel model);
    Task EditGameAsync(int gameId, GameModel model);
    Task DeleteGameAsync(int gameId);
    Task<int> GetGamesCountAsync(Filter filter = null);
}
