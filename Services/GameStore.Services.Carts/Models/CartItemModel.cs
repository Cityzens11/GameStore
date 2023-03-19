using AutoMapper;
using GameStore.Context.Entities;

namespace GameStore.Services.Carts;

public class CartItemModel
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string Name { get; set; }
    public string ImageUri { get; set; }

    public int GameId { get; set; }

    public int CartId { get; set; }
}



public class CartItemModelProfile : Profile
{
    public CartItemModelProfile()
    {
        CreateMap<CartItem, CartItemModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
