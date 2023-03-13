namespace GameStore.Web.Services;

public class User
{
    private readonly IHttpContextAccessor _contexAccessor;

    public User(IHttpContextAccessor context) 
    {
        _contexAccessor = context;
    }

    public string GetUserName() => _contexAccessor.HttpContext?.Request.Cookies["username"] ?? string.Empty;
    public bool IsSigned() => _contexAccessor.HttpContext?.Request.Cookies["token"] is null ? false : true;
}
