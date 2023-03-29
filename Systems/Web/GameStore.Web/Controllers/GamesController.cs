using AutoMapper;
using GameStore.Common.Filters;
using GameStore.Web.Models;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers;

[Route("Games")]
public class GamesController : Controller
{
    private IGameService _gameService;
    private ICommentService _commentService;
    private IMapper _mapper;
    private ICookieService _cookieService;

    public GamesController(
        IGameService gameService, 
        ICommentService commentService, 
        IMapper mapper, 
        ICookieService cookieService
        ) 
    {
        _gameService = gameService;
        _commentService = commentService;
        _mapper = mapper;
        _cookieService = cookieService;
    }

    [Route("GetGames")]
    public async Task<IActionResult> GetGames(int page = 1, Filter filter = null)
    {
        int pageSize = 12;

        var count = await _gameService.GetGamesCountAsync(filter);
        var games = await _gameService.GetGamesAsync((page - 1) * pageSize, pageSize, filter);

        PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
        IndexViewModel viewModel = new IndexViewModel(games, pageViewModel, filter);

        SetViewBag();
        return View(viewModel);
    }

    public async Task<IActionResult> GetGame(int gameId)
    {
        var game = await _gameService.GetGameAsync(gameId);
        var comments = await _commentService.GetCommentsAsync(gameId);

        var game_comments = new GameAndComments(game, comments);

        SetViewBag();
        return View(game_comments);
    }

    [HttpGet("AddGame")]
    public async Task<IActionResult> AddGame()
    {
        var model = new GameModel();

        SetViewBag();
        return View(model);
    }

    [HttpPost("AddGame")]
    public async Task<IActionResult> AddGame([FromForm] GameModel model)
    {
        if (ModelState.IsValid)
        {
            if(model.Genres != null)
            {
                ((List<string>)model.Genres).RemoveAll(x => x is null);
            }

            await _gameService.AddGameAsync(model);
            TempData["Add"] = "Game has been Added!";
            return RedirectToAction("GetGames");
        }

        SetViewBag();
        return View(model);
    }

    [HttpGet("UpdateGame")]
    public async Task<IActionResult> UpdateGame(int gameId)
    {
        var game = await _gameService.GetGameAsync(gameId);
        var model = _mapper.Map<GameModel>(game);

        SetViewBag();
        return View(model);
    }

    [HttpPost("UpdateGame")]
    public async Task<IActionResult> UpdateGame([FromForm] GameModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Genres != null)
            {
                ((List<string>)model.Genres).RemoveAll(x => x is null);
            }

            await _gameService.EditGameAsync((int)model.Id!, model);
            TempData["Add"] = "Game has been Updated!";
            return RedirectToAction("GetGames");
        }

        SetViewBag();
        return View(model);
    }

    [Route("DeleteGame")]
    public async Task<IActionResult> DeleteGame([FromQuery] int gameId)
    {
        await _gameService.DeleteGameAsync(gameId);
        TempData["Add"] = "Game has been Deleted!";

        return RedirectToAction("GetGames");
    }

    [NonAction]
    public async Task<int> GetGamesCountAsync()
    {
        var count = await _gameService.GetGamesCountAsync();
        return count;
    }

    [NonAction]
    public void SetViewBag()
    {
        ViewBag.IsSigned = _cookieService.IsSigned();
        ViewBag.UserName = _cookieService.GetUserName();
        ViewBag.UserRole = _cookieService.GetUserRole();
        ViewBag.FullName = _cookieService.GetFullName();
        ViewBag.Image = _cookieService.GetImage();
        ViewBag.CartId = _cookieService.GetCart();
        ViewBag.CartCount = _cookieService.GetCartSize();
    }
}
