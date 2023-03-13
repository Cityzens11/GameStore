namespace GameStore.Identity.Configuration;

using Duende.IdentityServer.Models;
using GameStore.Common.Security;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(AppScopes.Admin, "Access to games API - Manage all data"),
            new ApiScope(AppScopes.Manager, "Access to games API - Manage added data"),
            new ApiScope(AppScopes.Customer, "Access to games API - Order and comment data"),
        };
}
