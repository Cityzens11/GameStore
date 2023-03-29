using AutoMapper;
using GameStore.Api.Controllers.Models;
using GameStore.Common.Responses;
using GameStore.Common.Security;
using GameStore.Services.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers;

/// <summary>
/// Games controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/carts")]
//[Authorize]
[ApiController]
[ApiVersion("1.0")]
public class CartsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<CartsController> logger;
    private readonly ICartService cartService;

    public CartsController(IMapper mapper, ILogger<CartsController> logger, ICartService cartService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.cartService = cartService;
    }

    /// <summary>
    /// Creates a new cart
    /// </summary>
    /// <param name="userName">Name of a user</param>
    /// <returns>Cart Response</returns>
    [ProducesResponseType(typeof(CartResponse), 200)]
    [HttpGet("")]
    public async Task<CartResponse> AddCart([FromQuery] string userName)
    {
        var cartModel = await cartService.CreateCart(userName);
        var response = mapper.Map<CartResponse>(cartModel);

        return response;
    }

    /// <summary>
    /// Get user cart
    /// </summary>
    /// <param name="userId">Id of user</param>
    /// <response code="200">Cart Response</response>
    [ProducesResponseType(typeof(CartResponse), 200)]
    [HttpGet("{cartId}")]
    public async Task<CartResponse> GetCart([FromRoute] int cartId)
    {
        var cart = await cartService.GetCart(cartId);
        var response = mapper.Map<CartResponse>(cart);

        return response;
    }

    [HttpPost("")]
    [Authorize(Policy = AppScopes.AnyPolicy)]
    public async Task AddCartItem([FromBody] AddCartItemRequest request)
    {
        var model = mapper.Map<AddCartItemModel>(request);
        await cartService.AddCartItem(model);
    }

    [HttpPut("{itemId}")]
    [Authorize(Policy = AppScopes.AnyPolicy)]
    public async Task UpdateCartItem([FromRoute] int itemId, [FromBody] int quantity)
    {
        await cartService.UpdateCartItem(itemId, quantity);
    }

    [HttpDelete("{itemId}")]
    [Authorize(Policy = AppScopes.AnyPolicy)]
    public async Task<IActionResult> DeleteGame([FromRoute] int itemId)
    {
        await cartService.DeleteCartItem(itemId);

        return Ok();
    }

    /// <summary>
    /// Get number of cartitems in a cart in db
    /// </summary>
    /// <returns>number of games</returns>
    [ProducesResponseType(typeof(int), 200)]
    [HttpGet("count/{cartId}")]
    public async Task<int> GetCartItemsCount([FromRoute] int cartId)
    {
        int count = await cartService.GetCartItemsCount(cartId);
        return count;
    }
}
