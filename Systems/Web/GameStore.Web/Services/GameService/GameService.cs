namespace GameStore.Web.Services;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameStore.Common.Filters;
using GameStore.Web.Cloudinary;
using GameStore.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class GameService : IGameService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ICookieService _cookieService;
    private readonly HttpClient _httpClient;
    private readonly CloudinarySettings _cloudinarySettings;

    private readonly string Token;

    public GameService(
        IHttpContextAccessor httpContextAccessor, 
        ICloudinaryService cloudinaryService, 
        ICookieService cookieService,
        HttpClient httpClient, 
        CloudinarySettings cloudinarySettings
        )
    {
        _cloudinarySettings = cloudinarySettings;
        _httpClient = httpClient;
        _cloudinaryService = cloudinaryService;
        _cookieService = cookieService;
        _httpContextAccessor = httpContextAccessor;

        Token = _cookieService.GetToken();
    }

    public async Task<IEnumerable<GameListItem>> GetGamesAsync(int offset = 0, int limit = 12, Filter filter = null)
    {
        string url = string.Empty;

        if (filter.Name is null && filter.Genres is null)
        {
            url = $"{Settings.ApiRoot}/v1/games?offset={offset}&limit={limit}";
        }
        else
        {
            if (filter.Name != null && filter.Name.Length > 3)
            {
                url += $"Name={filter.Name}";
            }
            if (filter.Genres != null && filter.Genres.Any())
            {
                url += $"&Genres={string.Join("&", filter.Genres)}";
            }
            url += $"&offset={offset}&limit={limit}";

            url = $"{Settings.ApiRoot}/v1/games/filter?" + url;
        }
        
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<GameListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<GameListItem>();

        return data;
    }

    public async Task<GameListItem> GetGameAsync(int gameId)
    {
        string url = $"{Settings.ApiRoot}/v1/games/{gameId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<GameListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new GameListItem();

        return data;
    }

    public async Task AddGameAsync(GameModel model)
    {
        if (model.Image != null)
        {
            model.ImageUri = await _cloudinaryService.UploadCloudinary(model.Image, "games");
        }
        string url = $"{Settings.ApiRoot}/v1/games";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task EditGameAsync(int gameId, GameModel model)
    {
        DeleteString(model);
        string url = $"{Settings.ApiRoot}/v1/games/{gameId}";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.PutAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task DeleteGameAsync(int gameId)
    {
        string url = $"{Settings.ApiRoot}/v1/games/{gameId}";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task<int> GetGamesCountAsync(Filter filter = null)
    {
        string url = string.Empty;

        if (filter.Name is null && filter.Genres is null)
        {
            url = $"{Settings.ApiRoot}/v1/games/count";
        }
        else
        {
            if (filter.Name != null)
            {
                url += $"Name={filter.Name}";
            }
            if (filter.Genres != null && filter.Genres.Any())
            {
                url += $"&Genres={string.Join("&", filter.Genres)}";
            }

            url = $"{Settings.ApiRoot}/v1/games/count/filter?" + url;
        }

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<int>(content);

        return data;
    }

    private void DeleteString(GameModel model)
    {
        ((List<string>)model.Genres).RemoveAll(x => x.Equals("System.Collections.Generic.List`1[System.String]"));
    }
}
