using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using GameStore.Context.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GameStore.Identity.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;

    public ProfileService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim(JwtClaimTypes.Name, user.UserName),
            new Claim(JwtClaimTypes.Role, user.Role)
        };

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = (user != null);
    }
}