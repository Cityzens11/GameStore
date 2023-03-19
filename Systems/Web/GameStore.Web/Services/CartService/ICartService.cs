using GameStore.Web.Models;

namespace GameStore.Web.Services;

public interface ICartService
{
    Task<CartModel> CreateCartAsync(string userName);
    Task<CartModel> GetCartAsync(int cartId);
    Task AddCartItemAsync(AddCartItem model);
    Task UpdateCartItemAsync(int itemId, int quantity);
    Task DeleteCartItemAsync(int itemId);
    Task<int> GetCartItemsCountAsync(int cartId);
}
