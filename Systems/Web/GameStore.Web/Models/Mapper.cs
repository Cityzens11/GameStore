using AutoMapper;

namespace GameStore.Web.Models;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameListItem, GameModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
            .ForMember(dest => dest.Image, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUri, opt => opt.MapFrom(src => src.ImageUri))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
    }
}
