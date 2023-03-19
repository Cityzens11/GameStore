namespace GameStore.Web.Models;

public class AccountResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string UserRole { get; set; }
    public string UserName { get; set; }
    public string? ImageUri { get; set; }
    public string Email { get; set; }
    public int CartId { get; set; }
}
