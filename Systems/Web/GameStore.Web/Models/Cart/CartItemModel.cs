namespace GameStore.Web.Models;

public class CartItemModel
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string Title { get; set; }
    public string ImageUri { get; set; }

    public int GameId { get; set; }

    public int CartId { get; set; }
}
