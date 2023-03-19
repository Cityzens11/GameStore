using AutoMapper;
using GameStore.Services.Carts;

namespace GameStore.Api.Controllers.Models;

public class CartItemResponse
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string Title { get; set; }
    public string ImageUri { get; set; }

    public int GameId { get; set; }

    public int CartId { get; set; }
}

public class CartItemResponseProfile : Profile
{
    public CartItemResponseProfile()
    {
        CreateMap<CartItemModel, CartItemResponse>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
    }
}
