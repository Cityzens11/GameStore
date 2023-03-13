namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using GameStore.Services.Games;

public class GameResponse
{
    /// <summary>
    /// Game id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Game title
    /// </summary>
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }
}

public class GameResponseProfile : Profile
{
    public GameResponseProfile()
    {
        CreateMap<GameModel, GameResponse>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
    }
}
