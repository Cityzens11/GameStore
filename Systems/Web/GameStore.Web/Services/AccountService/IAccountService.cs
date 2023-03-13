using GameStore.Web.Models;

namespace GameStore.Web.Services;

public interface IAccountService
{
    Task<bool> RegisterAsync(AccountModel model);
    Task<string> SignInAsync(SignInModel model);
    Task<AccountResponse> GetUserAsync(string username);
}
