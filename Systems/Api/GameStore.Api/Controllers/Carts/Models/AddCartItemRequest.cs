using AutoMapper;
using GameStore.Services.Carts;

namespace GameStore.Api.Controllers.Models;

public class AddCartItemRequest
{
    public int Quantity { get; set; }
    public float Price { get; set; }

    public int GameId { get; set; }

    public int CartId { get; set; }
}

public class AddCartItemRequestProfile : Profile
{
    public AddCartItemRequestProfile()
    {
        CreateMap<AddCartItemRequest, AddCartItemModel>();
    }
}
