using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models;

public class SignInModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Password { get; set; }
}
