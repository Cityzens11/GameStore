namespace GameStore.Services.Games;

using GameStore.Context.Entities;
using AutoMapper;

public class GameModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }
}

public class GameModelProfile : Profile
{
    public GameModelProfile()
    {
        CreateMap<Game, GameModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(x => x.Name)));
    }
}
