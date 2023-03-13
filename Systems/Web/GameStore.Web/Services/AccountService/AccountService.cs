using GameStore.Web.Models;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GameStore.Web.Services;

public class AccountService : IAccountService
{
    private readonly ICloudinaryService _cloudinaryService;
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient, ICloudinaryService cloudinaryService)
    {
        _httpClient = httpClient;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<bool> RegisterAsync(AccountModel model)
    {
        if (model.Image != null)
        {
            model.ImageUri = await _cloudinaryService.UploadCloudinary(model.Image, "accounts");
        }

        string url = $"{Settings.ApiRoot}/v1/accounts";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<string> SignInAsync(SignInModel model)
    {
        string url = $"{Settings.IdentityRoot}/connect/token";

        var form = new Dictionary<string, string>
        {
            {"grant_type", "password" },
            {"client_id", Settings.ClientId},
            {"client_secret", Settings.ClientSecret},
            {"username", model.UserName},
            {"password", model.Password},
            {"aud", "api"},
            {"scope", "admin manager customer"},
        };

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(form)
        };

        var response = await _httpClient.SendAsync(tokenRequest);
        var content = await response.Content.ReadAsStringAsync();
        var tokenJson = JObject.Parse(content);
        if (!response.IsSuccessStatusCode)
        {
            return tokenJson.Last.Last.ToString();
        }

        var accessToken = tokenJson.Value<string>("access_token");

        return accessToken;
    }

    public async Task<AccountResponse> GetUserAsync(string userName)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/getuser?userName={userName}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<AccountResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AccountResponse();

        return data;
    }
}
