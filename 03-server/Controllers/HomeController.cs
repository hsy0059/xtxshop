using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet("banner")]
    public async Task<ActionResult<ApiResponse<List<BannerItem>>>> GetBanner([FromQuery] int distributionSite = 1)
    {
        var result = await _homeService.GetBannersAsync(distributionSite);
        return Ok(ApiResponse<List<BannerItem>>.Success(result));
    }

    [HttpGet("category/mutli")]
    public async Task<ActionResult<ApiResponse<List<CategoryItem>>>> GetCategory()
    {
        var result = await _homeService.GetCategoriesAsync();
        return Ok(ApiResponse<List<CategoryItem>>.Success(result));
    }

    [HttpGet("hot/mutli")]
    public async Task<ActionResult<ApiResponse<List<HotItem>>>> GetHot()
    {
        var result = await _homeService.GetHotRecommendsAsync();
        return Ok(ApiResponse<List<HotItem>>.Success(result));
    }

    [HttpGet("goods/guessLike")]
    public async Task<ActionResult<ApiResponse<PageResult<GuessItem>>>> GetGuessLike([FromQuery] PageParams pageParams)
    {
        var result = await _homeService.GetGuessLikeAsync(pageParams);
        return Ok(ApiResponse<PageResult<GuessItem>>.Success(result));
    }
}
