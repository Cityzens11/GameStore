using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models;

public class CommentModel
{
    public int Id { get; set; }
    [Required]
    public string User { get; set; }
    [Required]
    [MinLength(5)]
    [MaxLength(600)]
    public string Body { get; set; }
    [Required]
    public DateTime CommentedTime { get; set; }
    [Required]
    public int GameId { get; set; }
    public int? ParentCommentId { get; set; }
}
