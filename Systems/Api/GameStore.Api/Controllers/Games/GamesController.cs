namespace GameStore.Api.Controllers;

using AutoMapper;
using GameStore.Api.Controllers.Models;
using GameStore.Common.Filters;
using GameStore.Common.Responses;
using GameStore.Common.Security;
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
[Route("api/v{version:apiVersion}/games")]
//[Authorize]
[ApiController]
[ApiVersion("1.0")]
public class GamesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<GamesController> logger;
    private readonly IGameService gameService;

    public GamesController(IMapper mapper, ILogger<GamesController> logger, IGameService gameService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.gameService = gameService;
    }


    /// <summary>
    /// Get games
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of GameResponses</response>
    [ProducesResponseType(typeof(IEnumerable<GameResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<GameResponse>> GetGames([FromQuery] int offset = 0, [FromQuery] int limit = 12)
    {
        var games = await gameService.GetGames(offset, limit);
        var response = mapper.Map<IEnumerable<GameResponse>>(games);

        return response;
    }

    /// <summary>
    /// Get games by filter
    /// </summary>
    /// <param name="filter">Filters the response</param>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of GameResponses</response>
    [ProducesResponseType(typeof(IEnumerable<GameResponse>), 200)]
    [HttpGet("aaa")]
    public async Task<IEnumerable<GameResponse>> GetGames([FromQuery] Filter filter, [FromQuery] int offset = 0, [FromQuery] int limit = 12)
    {
        var games = await gameService.GetGames(filter, offset, limit);
        var response = mapper.Map<IEnumerable<GameResponse>>(games);

        return response;
    }

    /// <summary>
    /// Get number of games in db
    /// </summary>
    /// <returns>number of games</returns>
    [ProducesResponseType(typeof(int), 200)]
    [HttpGet("count")]
    public async Task<int> GetGamesCount()
    {
        int count = await gameService.GetGamesCount();
        return count;
    }

    /// <summary>
    /// Get number of games in db
    /// </summary>
    /// <returns>number of games</returns>
    [ProducesResponseType(typeof(int), 200)]
    [HttpGet("count/aaa")]
    public async Task<int> GetGamesCount([FromQuery] Filter filter)
    {
        int count = await gameService.GetGamesCount(filter);
        return count;
    }

    /// <summary>
    /// Get games by Id
    /// </summary>
    /// <response code="200">GameResponse></response>
    [ProducesResponseType(typeof(GameResponse), 200)]
    [HttpGet("{id}")]
    public async Task<GameResponse> GetGameById([FromRoute] int id)
    {
        var game = await gameService.GetGame(id);
        var response = mapper.Map<GameResponse>(game);

        return response;
    }

    [HttpPost("")]
    //[Authorize(Policy = AppScopes.Manager)]
    public async Task<GameResponse> AddGame([FromBody] AddGameRequest request)
    {
        var model = mapper.Map<AddGameModel>(request);
        var game = await gameService.AddGame(model);
        var response = mapper.Map<GameResponse>(game);

        return response;
    }

    [HttpPut("{id}")]
    //[Authorize(Policy = AppScopes.GamesWrite)]
    public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] UpdateGameRequest request)
    {
        var model = mapper.Map<UpdateGameModel>(request);
        await gameService.UpdateGame(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    //[Authorize(Policy = AppScopes.GamesWrite)]
    public async Task<IActionResult> DeleteGame([FromRoute] int id)
    {
        await gameService.DeleteGame(id);

        return Ok();
    }
}