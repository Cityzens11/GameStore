﻿using GameStore.Web.Models;
using System.Text.Json;
using System.Text;

namespace GameStore.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;

    public CartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CartModel> CreateCartAsync(string userName)
    {
        string url = $"{Settings.ApiRoot}/v1/carts?userName={userName}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<CartModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CartModel();

        return data;
    }

    public async Task<CartModel> GetCartAsync(int cartId)
    {
        string url = $"{Settings.ApiRoot}/v1/carts/{cartId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<CartModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CartModel();

        return data;
    }

    public async Task AddCartItemAsync(AddCartItem model)
    {
        string url = $"{Settings.ApiRoot}/v1/carts";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task UpdateCartItemAsync(int itemId, int quantity)
    {
        string url = $"{Settings.ApiRoot}/v1/carts/{itemId}";

        var body = JsonSerializer.Serialize(quantity);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task DeleteCartItemAsync(int itemId)
    {
        string url = $"{Settings.ApiRoot}/v1/carts/{itemId}";

        var response = await _httpClient.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task<int> GetCartItemsCountAsync(int cartId)
    {
        string url = $"{Settings.ApiRoot}/v1/carts/count/{cartId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<int>(content);

        return data;
    }
}
