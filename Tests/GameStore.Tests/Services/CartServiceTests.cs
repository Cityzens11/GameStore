using AutoMapper;
using GameStore.Context.Entities;
using GameStore.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using GameStore.Services.Carts;

namespace GameStore.Tests.Services;

public class CartServiceTests : IDisposable
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IDbContextFactory<MainDbContext>> contextFactoryMock;
    private readonly DbContextOptions<MainDbContext> _options;

    public CartServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        contextFactoryMock = new Mock<IDbContextFactory<MainDbContext>>();

        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
    }

    [Fact]
    public async Task CreateCart_ShouldCreateCartAndSetCartIdOnUser()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var userName = "Test UserName";
        var fullName = "Test FullName";
        var status = UserStatus.Active;
        var image = "Test Image";
        var role = "Test Role";
        var email = "Test Email";

        var user = new User 
        { 
            UserName = userName, 
            FullName = fullName, 
            Status = status, 
            ImageUri = image, 
            Role = role, 
            Email = email 
        };

        context.Users.Add(user);
        context.SaveChanges();

        mapperMock.Setup(x => x.Map<CartModel>(It.IsAny<Cart>())).Returns(
            (Cart cart) => new CartModel
            {
                Id = cart.Id,
                UserId = user.Id
            }
        );

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var cartService = new CartService(
            mapperMock.Object,
            contextFactoryMock.Object
            );

        // Act
        var result = await cartService.CreateCart(userName);

        // Assert
        using var cn = new MainDbContext(_options);
        Assert.NotNull(result);
        Assert.Equal(user.CartId, result.Id);
        Assert.Single(cn.Carts);
        Assert.Equal(user.Id, cn.Carts.First().UserId);
    }

    [Fact]
    public async Task GetCart_ShouldReturnCartWithCartItems()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var userName = "Test UserName";
        var fullName = "Test FullName";
        var status = UserStatus.Active;
        var image = "Test Image";
        var role = "Test Role";
        var email = "Test Email";

        var user = new User
        {
            UserName = userName,
            FullName = fullName,
            Status = status,
            ImageUri = image,
            Role = role,
            Email = email
        };

        context.Users.Add(user);
        context.SaveChanges();


        var cartItem = new CartItem
        {
            CartId = 1,
            Quantity = 1,
            Price = 2f,
            ImageUri = "Test Image",
            Name = "Test Name"
        };
        var cart = new Cart() 
        { 
            Id = 1,
            CartItems = new List<CartItem> { cartItem }
        };
        context.Carts.Add(cart);
        context.SaveChanges();

        mapperMock.Setup(x => x.Map<CartModel>(It.IsAny<Cart>())).Returns(
            (Cart cart) => new CartModel
            {
                Id = cart.Id,
                UserId = user.Id,
                CartItems = new List<CartItemModel> { new CartItemModel
                    {
                        CartId = cartItem.CartId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        ImageUri = cartItem.ImageUri,
                        Name = cartItem.Name
                    } 
                }
            }
        );

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var cartService = new CartService(
            mapperMock.Object,
            contextFactoryMock.Object
            );

        // Act
        var result = await cartService.GetCart(cart.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cart.Id, result.Id);
        Assert.Single(result.CartItems);
    }

    [Fact]
    public async Task AddCartItem_ShouldAddCartItemToCart()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var cart = new Cart();
        var game = new Game 
        { 
            Id = 1, 
            Title = "Test Title", 
            Description = "Test Description", 
            ImageUri = "Test Image", 
            Publisher = "Test Publisher" 
        }; 

        context.Carts.Add(cart);
        context.Games.Add(game);
        context.SaveChanges();

        mapperMock.Setup(x => x.Map<CartItem>(It.IsAny<AddCartItemModel>())).Returns(
            (AddCartItemModel model) => new CartItem
            {
                GameId = model.GameId,
                CartId = model.CartId,
                Quantity = model.Quantity,
                Price = model.Price
            });

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var cartService = new CartService(
            mapperMock.Object,
            contextFactoryMock.Object
            ); 

        var model = new AddCartItemModel 
        { 
            CartId = cart.Id, 
            GameId = game.Id,
        };

        // Act
        await cartService.AddCartItem(model);

        // Assert
        using var cn = new MainDbContext(_options);
        Assert.Single(cn.CartItems);
        Assert.Equal(1, cn.CartItems.First().Quantity);
        Assert.Equal(game.Price, cn.CartItems.First().Price);
        Assert.Equal(game.ImageUri, cn.CartItems.First().ImageUri);
        Assert.Equal(game.Title, cn.CartItems.First().Name);
    }

    [Fact]
    public async Task UpdateCartItem_UpdatesQuantity()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var cartItem = new CartItem { Id = 1, Quantity = 2, Price = 3f, ImageUri = "Test Image", Name = "Test Name" };
        context.CartItems.Add(cartItem);
        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var cartService = new CartService(
            mapperMock.Object,
            contextFactoryMock.Object
            );

        // Act
        await cartService.UpdateCartItem(itemId: 1, quantity: 3);

        // Assert
        using var cn = new MainDbContext(_options);
        var updatedCartItem = await cn.CartItems.FindAsync(1);
        Assert.Equal(3, updatedCartItem.Quantity);
    }

    [Fact]
    public async Task DeleteCartItem_RemovesItem()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var cartItem = new CartItem { Id = 1, Quantity = 2, Price = 3f, ImageUri = "Test Image", Name = "Test Name" };
        context.CartItems.Add(cartItem);
        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var cartService = new CartService(
            mapperMock.Object,
            contextFactoryMock.Object
            );

        // Act
        await cartService.DeleteCartItem(itemId: 1);

        // Assert
        using var cn = new MainDbContext(_options);
        var deletedCartItem = await cn.CartItems.FindAsync(1);
        Assert.Null(deletedCartItem);
    }

    [Fact]
    public async Task GetCartItemsCount_ReturnsCorrectCount()
    {
        // Arrange
        using var context = new MainDbContext(_options);

        var cartItem1 = new CartItem
        {
            CartId = 1,
            Quantity = 1,
            Price = 2f,
            ImageUri = "Test Image 1",
            Name = "Test Name 1"
        };
        var cartItem2 = new CartItem
        {
            CartId = 1,
            Quantity = 2,
            Price = 3f,
            ImageUri = "Test Image 2",
            Name = "Test Name 2"
        };
        var cart = new Cart()
        {
            Id = 1,
            CartItems = new List<CartItem> { cartItem1, cartItem2 }
        };
        context.Carts.Add(cart);
        context.SaveChanges();

        contextFactoryMock
            .Setup(x => x.CreateDbContextAsync(default(CancellationToken)))
            .ReturnsAsync(context);

        var cartService = new CartService(
            mapperMock.Object,
            contextFactoryMock.Object
            );

        // Act
        var count = await cartService.GetCartItemsCount(cartId: 1);

        // Assert
        Assert.Equal(2, count);
    }

    public void Dispose()
    {
        var context = new MainDbContext(_options);
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}

