namespace GameStore.Web.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _contexAccessor;

    public CookieService(IHttpContextAccessor context)
    {
        _contexAccessor = context;
    }

    public string GetUserName() => _contexAccessor.HttpContext?.Request.Cookies["username"] ?? string.Empty;
    public string GetFullName() => _contexAccessor.HttpContext?.Request.Cookies["fullname"] ?? string.Empty;
    public string GetImage() => _contexAccessor.HttpContext?.Request.Cookies["image"] ?? string.Empty;
    public bool IsSigned() => _contexAccessor.HttpContext?.Request.Cookies["token"] is null ? false : true;
}
