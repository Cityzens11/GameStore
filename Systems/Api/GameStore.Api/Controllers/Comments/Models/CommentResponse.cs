namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using GameStore.Services.Comments;

public class CommentResponse
{
    /// <summary>
    /// Comment id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Comment author
    /// </summary>
    public string User { get; set; }
    public string Body { get; set; }
    public DateTime CommentedTime { get; set; }
    public virtual int GameId { get; set; }
}

public class CommentResponseProfile : Profile
{
    public CommentResponseProfile()
    {
        CreateMap<CommentModel, CommentResponse>()
            .ForMember(dest => dest.CommentedTime, opt => opt.MapFrom(src => src.TimeStamp));
    }
}
