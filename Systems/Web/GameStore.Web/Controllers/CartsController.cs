using GameStore.Web.Models;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers;

[Route("Carts")]
public class CartsController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICookieService _cookieService;

    public CartsController(ICartService cartService, ICookieService cookieService)
    {
        _cartService = cartService;
        _cookieService = cookieService;
    }

    [HttpGet("GetCart")]
    public async Task<IActionResult> GetCart(int cartId)
    {
        var cart = await _cartService.GetCartAsync(cartId);

        SetViewBag();
        return View(cart);
    }

    [HttpGet("AddCart")]
    public async Task<IActionResult> AddCartItem(int cartId, int gameId)
    {
        var addItem = new AddCartItem { CartId = cartId, GameId = gameId };
        await _cartService.AddCartItemAsync(addItem);

        int cartSize = await _cartService.GetCartItemsCountAsync(cartId);
        _cookieService.SetCartSize(cartSize.ToString());

        return RedirectToAction("GetGames", "Games");
    }

    [HttpPost("UpdateCart")]
    public async Task<IActionResult> UpdateCartItem(int itemId, int quantity)
    {
        await _cartService.UpdateCartItemAsync(itemId, quantity);
        return RedirectToAction("GetGames", "Games");
    }

    [Route("DeleteCart")]
    public async Task<IActionResult> DeleteCartItem(int itemId)
    {
        await _cartService.DeleteCartItemAsync(itemId);

        int cartId = int.Parse(_cookieService.GetCart());
        int cartSize = await _cartService.GetCartItemsCountAsync(cartId);
        _cookieService.SetCartSize(cartSize.ToString());

        return RedirectToAction("GetCart", new { cartId = cartId });
    }

    [HttpGet("Proceed")]
    public async Task<IActionResult> Proceed()
    {
        var proceedModel = new ProceedModel();

        SetViewBag();
        return View(proceedModel);
    }

    [HttpPost("Proceed")]
    public async Task<IActionResult> Proceed([FromForm] ProceedModel model)
    {
        TempData["Message"] = "Form submitted successfully!";
        SetViewBag();
        return View("AfterProceed", model);
    }


    [NonAction]
    public void SetViewBag()
    {
        ViewBag.IsSigned = _cookieService.IsSigned();
        ViewBag.UserName = _cookieService.GetUserName();
        ViewBag.FullName = _cookieService.GetFullName();
        ViewBag.Image = _cookieService.GetImage();
        ViewBag.CartId = _cookieService.GetCart();
        ViewBag.CartCount = _cookieService.GetCartSize();
    }
}
