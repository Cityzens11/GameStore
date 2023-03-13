using System.ComponentModel.DataAnnotations;

namespace GameStore.Context.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    public int GameId { get; set; }

    public virtual int AccountId { get; set; }
    public virtual Account Account { get; set; }
}
