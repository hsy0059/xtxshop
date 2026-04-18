using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GoodsController : ControllerBase
{
    private readonly IGoodsService _goodsService;

    public GoodsController(IGoodsService goodsService)
    {
        _goodsService = goodsService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<GoodsResult>>> GetGoods([FromQuery] string id)
    {
        var result = await _goodsService.GetGoodsByIdAsync(id);
        if (result == null)
        {
            return NotFound(ApiResponse<GoodsResult>.Error("商品不存在"));
        }
        return Ok(ApiResponse<GoodsResult>.Success(result));
    }

    [HttpGet("{id}/relevant")]
    public async Task<ActionResult<ApiResponse<List<GoodsItem>>>> GetRelevant(string id)
    {
        var result = await _goodsService.GetSimilarProductsAsync(id);
        return Ok(ApiResponse<List<GoodsItem>>.Success(result));
    }
}
