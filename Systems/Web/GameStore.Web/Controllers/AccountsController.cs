using GameStore.Web.Models;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers;

[Route("Accounts")]
public class AccountsController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ICookieService _cookieService;

    public AccountsController(IAccountService accountService, ICookieService cookieService)
    {
        _accountService = accountService;
        _cookieService = cookieService;
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

        bool ok = await _accountService.RegisterAsync(model);
        if(!ok)
        {
            TempData["temp"] = "Could not create new account\nCheck input values for validity";
            return View(model);
        }

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

        Response.Cookies.Append("token", accessToken, new CookieOptions
        {
            Expires = remember ? DateTimeOffset.Now.AddMinutes(20) : DateTimeOffset.Now.AddDays(7)
        });
        Response.Cookies.Append("username", user.UserName, new CookieOptions
        {
            Expires = remember ? DateTimeOffset.Now.AddMinutes(20) : DateTimeOffset.Now.AddDays(7)
        });
        Response.Cookies.Append("fullname", user.FullName, new CookieOptions
        {
            Expires = remember ? DateTimeOffset.Now.AddMinutes(20) : DateTimeOffset.Now.AddDays(7)
        });
        Response.Cookies.Append("image", user.ImageUri, new CookieOptions
        {
            Expires = remember ? DateTimeOffset.Now.AddMinutes(20) : DateTimeOffset.Now.AddDays(7)
        });

        return RedirectToAction("GetGames", "Games");
    }

    [HttpGet("SignOut")]
    public new IActionResult SignOut()
    {
        if (!_cookieService.IsSigned())
            return BadRequest();

        Response.Cookies.Delete("token");
        Response.Cookies.Delete("username");
        return RedirectToAction("GetGames", "Games");
    }
}
