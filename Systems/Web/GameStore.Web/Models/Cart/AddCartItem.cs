namespace GameStore.Web.Models;

public class AddCartItem
{
    public int Quantity { get; set; }
    public float Price { get; set; }

    public int GameId { get; set; }

    public int CartId { get; set; }
}
