namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using GameStore.Services.UserAccount;

public class UserAccountResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string UserRole { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}

public class UserAccountResponseProfile : Profile
{
    public UserAccountResponseProfile()
    {
        CreateMap<UserAccountModel, UserAccountResponse>();
    }
}