using AutoMapper;
using GameStore.Context.Entities;

namespace GameStore.Services.Carts;

public class AddCartItemModel
{
    public int Quantity { get; set; }
    public float Price { get; set; }

    public int GameId { get; set; }

    public int CartId { get; set; }
}

public class AddCartItemModelProfile : Profile
{
    public AddCartItemModelProfile()
    {
        CreateMap<AddCartItemModel, CartItem>();
    }
}