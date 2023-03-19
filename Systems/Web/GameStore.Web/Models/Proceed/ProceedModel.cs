using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models;

public class ProceedModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Payment { get; set; }
    [MaxLength(600)]
    public string? Comments { get; set; }
}
