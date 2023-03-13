using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models;

public class AccountModel
{
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string UserRole { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Password { get; set; }
}
