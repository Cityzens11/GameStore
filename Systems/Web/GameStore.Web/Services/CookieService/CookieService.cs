using AutoMapper.Execution;
using GameStore.Web.Models;

namespace GameStore.Web.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _contexAccessor;

    public CookieService(IHttpContextAccessor context)
    {
        _contexAccessor = context;
    }

    public void SetCookies(string[] names, string[] values, bool remember)
    {
        if (names.Length.Equals(values.Length))
        {
            for (int i = 0; i < names.Length; i++)
            {
                _contexAccessor.HttpContext?.Response.Cookies.Append(names[i], values[i], new CookieOptions
                {
                    Expires = remember ? DateTimeOffset.Now.AddDays(7) : DateTimeOffset.Now.AddMinutes(20)
                });
            }
        }
    }
    public string[] GetKeys() => new string[] { "token", "username", "fullname", "image", "cart_id", "carts_count" };
    public string[] GetValues(AccountResponse accountResponse, string token, int count)
    {
        return new string[] { token, accountResponse.UserName, accountResponse.FullName, accountResponse.ImageUri, accountResponse.CartId.ToString(), count.ToString() };
    }

    public string GetUserName() => _contexAccessor.HttpContext?.Request.Cookies["username"] ?? string.Empty;
    public string GetFullName() => _contexAccessor.HttpContext?.Request.Cookies["fullname"] ?? string.Empty;
    public string GetImage() => _contexAccessor.HttpContext?.Request.Cookies["image"] ?? string.Empty;
    public string GetCart() => _contexAccessor.HttpContext?.Request.Cookies["cart_id"] ?? string.Empty;
    public string GetCartSize() => _contexAccessor.HttpContext?.Request.Cookies["carts_count"] ?? string.Empty;
    public bool IsSigned() => _contexAccessor.HttpContext?.Request.Cookies["token"] is null ? false : true;

    public void SetCartSize(string size) => _contexAccessor.HttpContext?.Response.Cookies.Append("carts_count", size, new CookieOptions
    {
        Expires = DateTimeOffset.Now.AddDays(7)
    });


    public void DeleteCookies()
    {
        _contexAccessor.HttpContext?.Response.Cookies.Delete("token");
        _contexAccessor.HttpContext?.Response.Cookies.Delete("username");
        _contexAccessor.HttpContext?.Response.Cookies.Delete("fullname");
        _contexAccessor.HttpContext?.Response.Cookies.Delete("image");
        _contexAccessor.HttpContext?.Response.Cookies.Delete("cart_id");
    }
}
