namespace GameStore.Context.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; }
}
