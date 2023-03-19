namespace GameStore.Services.UserAccount;

using AutoMapper;
using GameStore.Common.Exceptions;
using GameStore.Common.Validator;
using GameStore.Context;
using GameStore.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using RestSharp.Authenticators;

public class UserAccountService : IUserAccountService
{
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;
    private readonly IDbContextFactory<MainDbContext> contextFactory;

    public UserAccountService(
        IMapper mapper,
        UserManager<User> userManager,
        IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator,
        IDbContextFactory<MainDbContext> contextFactory)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.registerUserAccountModelValidator = registerUserAccountModelValidator;
        this.contextFactory = contextFactory;
    }

    public async Task<UserAccountModel> GetUser(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is null)
            throw new ProcessException("User account does not exist");

        return mapper.Map<UserAccountModel>(user);
    }

    public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
    {
        registerUserAccountModelValidator.Check(model);
        
        //Find user by email
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new ProcessException($"User account with email {model.Email} already exist.");

        // Create user account
        user = new User()
        {
            Role = model.UserRole,
            Status = UserStatus.Active,
            FullName = model.FirstName + " " + model.LastName,
            ImageUri = model.ImageUri ?? string.Empty,
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = false,
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
        };
        
        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ProcessException($"Creating user account is wrong. {String.Join(", ", result.Errors.Select(s => s.Description))}");


        // Returning the created user
        return mapper.Map<UserAccountModel>(user);
    }

    public async Task SendVerifyEmail(UserAccountModel model)
    {
        var unverifiedUser = userManager.FindByEmailAsync(model.Email).Result;
        var code = await userManager.GenerateEmailConfirmationTokenAsync(unverifiedUser!);
        var email_body = "Please confirm your email address by copying this url:\n";
        var callback_url = $"http://localhost:5062/api/v1/accounts/confirmemail?userID={unverifiedUser!.Id.ToString()}&code={code}";
        var body = email_body + callback_url;

        RestClient client = new RestClient("https://api.mailgun.net/v3");
        client.Authenticator = new HttpBasicAuthenticator("api", "");

        var request = new RestRequest();
        request.AddParameter("domain", "", ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", ">");
        request.AddParameter("to", unverifiedUser.Email);
        request.AddParameter("subject", "Verification Email");
        request.AddParameter("text", body);
        request.Method = Method.Post;

        var response = client.Execute(request);
    }

    public async Task Verify(string userId, string code)
    {
        if (userId == null || code == null)
            throw new ProcessException("Invalid email confirmation url");

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ProcessException("Invalid email parameter");

        code = code.Replace(' ', '+');

        var result = await userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
            throw new ProcessException("Cannot confirm email");
    }
}
