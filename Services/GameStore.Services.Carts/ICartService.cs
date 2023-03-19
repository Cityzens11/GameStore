namespace GameStore.Services.Carts;

public interface ICartService
{
    Task<CartModel> CreateCart(string userName);
    Task<CartModel> GetCart(int cartId);
    Task AddCartItem(AddCartItemModel model);
    Task UpdateCartItem(int itemId, int quantity);
    Task DeleteCartItem(int itemId);
    Task<int> GetCartItemsCount(int cartId);
}
