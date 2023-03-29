using GameStore.Web.Models;

namespace GameStore.Web.Services;

public interface ICookieService
{
    public void SetCookies(string[] names, string[] values, bool remember);
    public string[] GetKeys();
    public string[] GetValues(AccountResponse accountResponse, string token, int count);

    public string GetUserName();
    public string GetFullName();
    public string GetUserRole();
    public string GetImage();
    public string GetCart();
    public string GetCartSize();
    public string GetToken();
    public void SetCartSize(string size);
    public bool IsSigned();

    public void DeleteCookies();
}
