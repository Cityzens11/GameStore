using AutoMapper;
using GameStore.Common.Filters;
using GameStore.Common.Validator;
using GameStore.Web.Models;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Reflection.Metadata.Ecma335;

namespace GameStore.Web.Controllers;

[Route("Games")]
public class GamesController : Controller
{
    private IGameService _gameService;
    private ICommentService _commentService;
    private IMapper _mapper;
    private User _user;

    public GamesController(IGameService gameService, ICommentService commentService, IMapper mapper, User user) 
    {
        _gameService = gameService;
        _commentService = commentService;
        _mapper = mapper;
        _user = user;
    }

    [Route("GetGames")]
    public async Task<IActionResult> GetGames(int page = 1, Filter filter = null)
    {
        int pageSize = 12;

        var count = await _gameService.GetGamesCountAsync(filter);
        var games = await _gameService.GetGamesAsync((page - 1) * pageSize, pageSize, filter);

        PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
        IndexViewModel viewModel = new IndexViewModel(games, pageViewModel, filter);

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
        return View(viewModel);
    }

    public async Task<IActionResult> GetGame(int gameId)
    {
        var game = await _gameService.GetGameAsync(gameId);
        var comments = await _commentService.GetCommentsAsync(gameId);

        var game_comments = new GameAndComments(game, comments);

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
        return View(game_comments);
    }

    [HttpGet("AddGame")]
    public async Task<IActionResult> AddGame()
    {
        var model = new GameModel();

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
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

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
        return View(model);
    }

    [HttpGet("UpdateGame")]
    public async Task<IActionResult> UpdateGame(int gameId)
    {
        var game = await _gameService.GetGameAsync(gameId);
        var model = _mapper.Map<GameModel>(game);

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
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

        ViewBag.IsSigned = _user.IsSigned();
        ViewBag.UserName = _user.GetUserName();
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
}
