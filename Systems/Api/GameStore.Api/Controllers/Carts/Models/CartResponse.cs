using AutoMapper;
using GameStore.Services.Carts;

namespace GameStore.Api.Controllers.Models;

public class CartResponse
{
    public int Id { get; set; }
    public Guid UserId { get; set; }

    public virtual ICollection<CartItemResponse> CartItems { get; set; }
}

public class CartResponseProfile : Profile
{
    public CartResponseProfile()
    {
        CreateMap<CartModel, CartResponse>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
    }
}


