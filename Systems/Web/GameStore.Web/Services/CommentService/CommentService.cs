namespace GameStore.Web.Services;

using GameStore.Web.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class CommentService : ICommentService
{
    private readonly HttpClient _httpClient;
    private readonly ICookieService _cookieService;

    private readonly string Token;

    public CommentService(HttpClient httpClient, ICookieService cookieService)
    {
        _httpClient = httpClient;
        _cookieService = cookieService;

        Token = _cookieService.GetToken();
    }

    public async Task<IEnumerable<CommentListItem>> GetCommentsAsync(int gameId)
    {
        string url = $"{Settings.ApiRoot}/v1/comments?gameId={gameId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<CommentListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CommentListItem>();

        return data;
    }

    public async Task<CommentListItem> GetCommentAsync(int commentId)
    {
        string url = $"{Settings.ApiRoot}/v1/comments/{commentId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<CommentListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CommentListItem();

        return data;
    }

    public async Task AddCommentAsync(CommentModel model)
    {
        string url = $"{Settings.ApiRoot}/v1/comments";

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

    public async Task EditCommentAsync(CommentModel model)
    {
        string url = $"{Settings.ApiRoot}/v1/comments/{model.Id}";

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

    public async Task DeleteCommentAsync(int commentId)
    {
        string url = $"{Settings.ApiRoot}/v1/comments/{commentId}";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }
}

