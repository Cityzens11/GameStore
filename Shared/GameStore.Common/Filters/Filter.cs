namespace GameStore.Common.Filters;

public class Filter
{
    public string Name { get; set; }
    public IEnumerable<string> Genres { get; set;}
}
