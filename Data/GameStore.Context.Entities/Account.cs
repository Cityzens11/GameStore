namespace GameStore.Context.Entities;

public class Account : BaseEntity
{
    public string NickName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Favourite> Favourites { get; set; }
}
