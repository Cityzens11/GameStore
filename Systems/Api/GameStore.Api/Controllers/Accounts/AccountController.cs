namespace GameStore.Api.Controllers;

using AutoMapper;
using GameStore.Api.Controllers.Models;
using GameStore.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;

[Route("api/v{version:apiVersion}/accounts")]
[ApiController]
[ApiVersion("1.0")]
public class AccountsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<AccountsController> logger;
    private readonly IUserAccountService userAccountService;

    public AccountsController(IMapper mapper, ILogger<AccountsController> logger, IUserAccountService userAccountService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.userAccountService = userAccountService;
    }

    [HttpPost("")]
    public async Task<UserAccountResponse> Register([FromBody] RegisterUserAccountRequest request)
    {
        var user = await userAccountService.Create(mapper.Map<RegisterUserAccountModel>(request));

        await userAccountService.SendVerifyEmail(user);

        var response = mapper.Map<UserAccountResponse>(user);

        return response;
    }

    [HttpGet("confirmemail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        await userAccountService.Verify(userId, code);
        return Ok("Email confirmed");
    }
}