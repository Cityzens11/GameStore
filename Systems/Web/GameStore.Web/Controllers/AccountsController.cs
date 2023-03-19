using GameStore.Web.Models;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers;

[Route("Accounts")]
public class AccountsController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ICookieService _cookieService;
    private readonly ICartService _cartService;

    public AccountsController(IAccountService accountService, ICookieService cookieService, ICartService cartService)
    {
        _accountService = accountService;
        _cookieService = cookieService;
        _cartService = cartService;
    }

    [HttpGet("Register")]
    public async Task<IActionResult> Register()
    {
        if (_cookieService.IsSigned())
            return BadRequest();

        var account = new AccountModel();

        return View(account);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] AccountModel model)
    {
        if (_cookieService.IsSigned())
            return BadRequest();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!await _accountService.RegisterAsync(model))
        {
            TempData["temp"] = "Could not create new account\nCheck input values for validity";
            return View(model);
        }

        await _cartService.CreateCartAsync(model.UserName);

        TempData["temp"] = "Check your email for further instructions";
        return RedirectToAction("GetGames", "Games");
    }
    
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromForm] SignInModel model)
    {
        if (_cookieService.IsSigned())
            return BadRequest();

        var remember = Request.Form["Remember"].Equals("on")? true: false;
        if (!ModelState.IsValid)
        {
            TempData["temp"] = "Invalid Data Input";
            RedirectToAction("GetGames", "Games");
        }

        var accessToken = await _accountService.SignInAsync(model);
        if (!Settings.JwtTokenPattern.IsMatch(accessToken)) 
        {
            TempData["temp"] = accessToken;
            return RedirectToAction("GetGames", "Games");
        }

        var user = await _accountService.GetUserAsync(model.UserName);
        var count = await _cartService.GetCartItemsCountAsync(user.CartId);

        var keys =  _cookieService.GetKeys();
        var values = _cookieService.GetValues(user, accessToken, count);
        _cookieService.SetCookies(keys, values, remember);

        return RedirectToAction("GetGames", "Games");
    }

    [HttpGet("SignOut")]
    public new IActionResult SignOut()
    {
        if (!_cookieService.IsSigned())
            return BadRequest();

        _cookieService.DeleteCookies();
        return RedirectToAction("GetGames", "Games");
    }
}
