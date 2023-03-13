namespace GameStore.Web.Services;

public interface ICookieService
{
    public string GetUserName();
    public string GetFullName();
    public string GetImage();
    public bool IsSigned();
}
