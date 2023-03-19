namespace GameStore.Context.Entities;

public class CartItem : BaseEntity
{
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string ImageUri { get; set; }
    public string Name { get; set; }

    public int GameId { get; set; }
    public virtual Game Game { get; set; }

    public int CartId { get; set; }
    public virtual Cart Cart { get; set; }
}
