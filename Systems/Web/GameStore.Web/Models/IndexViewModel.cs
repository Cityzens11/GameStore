using GameStore.Common.Filters;

namespace GameStore.Web.Models;

public class IndexViewModel
{
    public IEnumerable<GameListItem> Games { get; }
    public PageViewModel PageViewModel { get; }
    public Filter Filter { get; set; }

    public IndexViewModel(IEnumerable<GameListItem> games, PageViewModel viewModel, Filter filter)
    {
        Games = games;
        PageViewModel = viewModel;
        Filter = filter;
    }
}
