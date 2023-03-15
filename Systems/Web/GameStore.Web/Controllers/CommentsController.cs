using AutoMapper;
using GameStore.Web.Models;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers;

[Route("Comments")]
public class CommentsController : Controller
{
    private IGameService _gameService;
    private ICommentService _commentService;
    private IMapper _mapper;
    private ICookieService _cookieService;

    public CommentsController(
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

    [HttpPost("Add")]
    public async Task<IActionResult> AddComment([FromForm] CommentModel model)
    {
        if (ModelState.IsValid)
        {
            TempData["validation"] = "Comment has been added";
            await _commentService.AddCommentAsync(model);
        }
        else
        {
            TempData["validation"] = "Not Valid Comment\nMin length - 5, Max length - 600 characters";
        }

        return RedirectToAction("GetGame", "Games", new { gameId = model.GameId });
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> EditComment([FromForm] CommentModel model)
    {
        if (ModelState.IsValid)
        {
            TempData["validation"] = "Comment has been edited";
            await _commentService.EditCommentAsync(model);
        }
        else
        {
            TempData["validation"] = "Not Valid Comment\nMin length - 5, Max length - 600 characters";
        }

        return RedirectToAction("GetGame", "Games", new { gameId = model.GameId });
    }

    [Route("Delete")]
    public async Task<IActionResult> DeleteComment(int commentId, int gameId)
    {
        await _commentService.DeleteCommentAsync(commentId);
        TempData["delete"] = "Comment has been Deleted!";

        return RedirectToAction("GetGame", "Games", new { gameId = gameId });
    }
}
