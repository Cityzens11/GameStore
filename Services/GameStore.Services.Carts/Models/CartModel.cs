using AutoMapper;
using GameStore.Context.Entities;

namespace GameStore.Services.Carts;

public class CartModel
{
    public int Id { get; set; }
    public Guid UserId { get; set; }

    public virtual ICollection<CartItemModel> CartItems { get; set; }
}

public class GameModelProfile : Profile
{
    public GameModelProfile()
    {
        CreateMap<Cart, CartModel>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
    }
}