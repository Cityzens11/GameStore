namespace GameStore.Context.Entities;

public class Game : BaseEntity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public float Price { get; set; }

    public string Publisher { get; set; }

    public string ImageUri { get; set; }

    public virtual ICollection<Genre> Genres { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
}
