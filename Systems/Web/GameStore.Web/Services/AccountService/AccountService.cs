using GameStore.Web.Models;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace GameStore.Web.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RegisterAsync(AccountModel model)
    {
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
}
