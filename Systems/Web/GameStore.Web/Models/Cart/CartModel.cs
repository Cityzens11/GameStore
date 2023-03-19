namespace GameStore.Web.Models;

public class CartModel
{
    public int Id { get; set; }
    public Guid UserId { get; set; }

    public virtual ICollection<CartItemModel> CartItems { get; set; }
}
