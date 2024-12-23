using Microsoft.AspNetCore.Mvc;
using RtlTvMaze.Model;
using RtlTvMaze.Services;

namespace RtlTvMaze.Controllers;

[ApiController]
[Route("[controller]")]
public class TvShowsController : ControllerBase
{
    private readonly ITvShowService _tvShowService;

    public TvShowsController(ITvShowService tvShowService)
    {
        _tvShowService = tvShowService;
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] int page=1, [FromQuery] int size=10)
    {
        PagedList<TvShowModel> value = await _tvShowService.Get(searchTerm, page, size);
        return Ok(value);
    }
}
