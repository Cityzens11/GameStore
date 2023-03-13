using System.ComponentModel.DataAnnotations;

namespace GameStore.Context.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Game> Games { get; set; }
}
