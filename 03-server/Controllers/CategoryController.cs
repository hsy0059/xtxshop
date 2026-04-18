using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IGoodsService _goodsService;

    public CategoryController(IGoodsService goodsService)
    {
        _goodsService = goodsService;
    }

    [HttpGet("top")]
    public async Task<ActionResult<ApiResponse<List<CategoryTopItem>>>> GetTop()
    {
        var result = await _goodsService.GetCategoryTopAsync();
        return Ok(ApiResponse<List<CategoryTopItem>>.Success(result));
    }
}
