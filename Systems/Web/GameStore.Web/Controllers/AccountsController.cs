using GameStore.Common.Filters;
using GameStore.Web.Models;
using GameStore.Web.Models.Account;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers;

[Route("Accounts")]
public class AccountsController : Controller
{
    private readonly IAccountService _accountService;
    private readonly User _user;

    public AccountsController(IAccountService accountService, User user)
    {
        _accountService = accountService;
        _user = user;
    }

    [HttpGet("Register")]
    public async Task<IActionResult> Register()
    {
        var account = new AccountModel();

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
        return View(account);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] AccountModel model)
    {
        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
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
    
    [HttpPost]
    public async Task<IActionResult> SignIn([FromForm] SignInModel model)
    {
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
        

        Response.Cookies.Append("token", accessToken, new CookieOptions
        {
            Expires = remember ? DateTimeOffset.Now.AddMinutes(20) : DateTimeOffset.Now.AddDays(7)
        });
        Response.Cookies.Append("username", model.UserName, new CookieOptions
        {
            Expires = remember ? DateTimeOffset.Now.AddMinutes(20) : DateTimeOffset.Now.AddDays(7)
        });

        return RedirectToAction("GetGames", "Games");
    }

    public new IActionResult SignOut()
    {
        Response.Cookies.Delete("token");
        Response.Cookies.Delete("username");
        return RedirectToAction("GetGames", "Games");
    }
}
