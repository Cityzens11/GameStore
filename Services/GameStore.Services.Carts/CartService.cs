namespace GameStore.Services.Carts;

using AutoMapper;
using GameStore.Common.Exceptions;
using GameStore.Context;
using GameStore.Context.Entities;
using Microsoft.EntityFrameworkCore;


public class CartService : ICartService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<MainDbContext> contextFactory;

    public CartService(
        IMapper mapper,
        IDbContextFactory<MainDbContext> contextFactory
        )
    {
        this.mapper = mapper;
        this.contextFactory = contextFactory;
    }

    public async Task<CartModel> CreateCart(string userName)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(userName))
            ?? throw new ProcessException($"User with User Name {userName} was not found");

        var cart = new Cart() { UserId = user.Id };
        context.Carts.Add(cart);
        context.SaveChanges();

        var temp = context.Carts.FirstOrDefault(c => c.UserId == user.Id);

        user.CartId = temp?.Id;
        context.Users.Update(user);
        context.SaveChanges();

        return mapper.Map<CartModel>(cart);
    }

    public async Task<CartModel> GetCart(int cartId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var cart = await context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id.Equals(cartId));

        var data = mapper.Map<CartModel>(cart);

        return data;
    }

    public async Task AddCartItem(AddCartItemModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var cart = await context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id.Equals(model.CartId))
            ?? throw new ProcessException($"Cart with ID {model.CartId} was not found");

        var game = await context.Games.FirstOrDefaultAsync(x => x.Id.Equals(model.GameId))
            ?? throw new ProcessException($"Game with Id {model.GameId} was not found");

        var cartItem = cart.CartItems.FirstOrDefault(x => x.GameId.Equals(model.GameId));

        if (cartItem != null)
        {
            cartItem.Quantity++;
            context.CartItems.Update(cartItem);
        }
        else
        {
            model.Price = game.Price;
            model.Quantity = 1;
            var addItem = mapper.Map<CartItem>(model);
            addItem.ImageUri = game.ImageUri;
            addItem.Name = game.Title;

            cart.CartItems.Add(addItem);
            context.Carts.Update(cart);
        }

        context.SaveChanges();
    }

    public async Task UpdateCartItem(int itemId, int quantity)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var item = await context.CartItems.FirstOrDefaultAsync(x => x.Id.Equals(itemId))
            ?? throw new ProcessException($"CartItem with ID {itemId} was not found");

        item.Quantity = quantity;
        context.CartItems.Update(item);
        context.SaveChanges();
    }

    public async Task DeleteCartItem(int itemId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var cartItem = await context.CartItems.FirstOrDefaultAsync(x => x.Id.Equals(itemId))
            ?? throw new ProcessException($"CartItem with ID {itemId} was not found");

        context.CartItems.Remove(cartItem);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetCartItemsCount(int cartId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var cart = await context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(x => x.Id.Equals(cartId))
            ?? throw new ProcessException($"Cart with ID {cartId} was not found");

        var count = cart.CartItems.Count;

        return count;
    }
}
