namespace GameStore.Context.Entities;

public class Comment : BaseEntity
{
    public string Author { get; set; }

    public string Body { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual int GameId { get; set; }
    public virtual Game Game { get; set; }
}
