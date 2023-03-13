using Microsoft.AspNetCore.Identity;

namespace GameStore.Context.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public UserStatus Status { get; set; }
    public string ImageUri { get; set; }
    public string Role { get; set; }
}
