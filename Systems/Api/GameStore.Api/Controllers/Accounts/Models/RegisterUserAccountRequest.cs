namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using FluentValidation;
using GameStore.Services.UserAccount;

public class RegisterUserAccountRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserRole { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ImageUri { get; set; }
}

public class RegisterUserAccountRequestValidator : AbstractValidator<RegisterUserAccountRequest>
{
    public RegisterUserAccountRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(50).WithMessage("Password is long.");
    }
}

public class RegisterUserAccountRequestProfile : Profile
{
    public RegisterUserAccountRequestProfile()
    {
        CreateMap<RegisterUserAccountRequest, RegisterUserAccountModel>();
    }
}

