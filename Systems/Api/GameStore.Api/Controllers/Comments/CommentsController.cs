namespace GameStore.Api.Controllers;

using AutoMapper;
using GameStore.Api.Controllers.Models;
using GameStore.Common.Responses;
using GameStore.Common.Security;
using GameStore.Services.Comments;
using GameStore.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Games controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/comments")]
//[Authorize]
[ApiController]
[ApiVersion("1.0")]
public class CommentsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<CommentsController> logger;
    private readonly ICommentService commentService;

    public CommentsController(IMapper mapper, ILogger<CommentsController> logger, ICommentService commentService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.commentService = commentService;
    }


    /// <summary>
    /// Get Comments
    /// </summary>
    /// <param name="gameId">Id of the game</param>
    /// <response code="200">List of CommentResponses</response>
    [ProducesResponseType(typeof(IEnumerable<CommentResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<CommentResponse>> GetComments([FromQuery] int gameId)
    {
        var comments = await commentService.GetComments(gameId);
        var response = mapper.Map<IEnumerable<CommentResponse>>(comments);

        return response;
    }

    /// <summary>
    /// Get Comments by Id
    /// </summary>
    /// <response code="200">CommentResponse></response>
    [ProducesResponseType(typeof(CommentResponse), 200)]
    //[Authorize(Policy = AppScopes.GamesRead)]
    [HttpGet("{id}")]
    public async Task<CommentResponse> GetCommentById([FromRoute] int id)
    {
        var comment = await commentService.GetComment(id);
        var response = mapper.Map<CommentResponse>(comment);

        return response;
    }

    [HttpPost("")]
    //[Authorize(Policy = AppScopes.GamesWrite)]
    public async Task<CommentResponse> AddComment([FromBody] AddCommentRequest request)
    {
        var model = mapper.Map<AddCommentModel>(request);
        var comment = await commentService.AddComment(model);
        var response = mapper.Map<CommentResponse>(comment);

        return response;
    }

    [HttpPut("{id}")]
    //[Authorize(Policy = AppScopes.GamesWrite)]
    public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequest request)
    {
        var model = mapper.Map<UpdateCommentModel>(request);
        await commentService.UpdateComment(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    //[Authorize(Policy = AppScopes.GamesWrite)]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        await commentService.DeleteComment(id);

        return Ok();
    }
}
