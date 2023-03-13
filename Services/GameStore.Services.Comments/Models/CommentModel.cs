namespace GameStore.Services.Comments;

using GameStore.Context.Entities;
using AutoMapper;

public class CommentModel
{
    public int Id { get; set; }
    public string User { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public int GameId { get; set; }
}

public class CommentModelProfile : Profile
{
    public CommentModelProfile()
    {
        CreateMap<Comment, CommentModel>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Author));
    }
}
