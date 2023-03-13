namespace GameStore.Services.UserAccount;

using AutoMapper;
using GameStore.Context.Entities;

public class UserAccountModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string UserRole { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}

public class UserAccountModelProfile : Profile
{
    public UserAccountModelProfile()
    {
        CreateMap<User, UserAccountModel>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
            .ForMember(d => d.FullName, d => d.MapFrom(d => d.FullName))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.UserRole, o => o.MapFrom(s => s.Role))
            ;
    }
}
