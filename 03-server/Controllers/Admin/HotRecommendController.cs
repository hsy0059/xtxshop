using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Controllers.Admin;

[ApiController]
[Route("admin/[controller]")]
public class HotRecommendController : ControllerBase
{
    private readonly AppDbContext _context;

    public HotRecommendController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<HotRecommend>>>> GetList()
    {
        var hots = await _context.HotRecommends
            .OrderBy(h => h.Sort)
            .ToListAsync();
        return Ok(ApiResponse<List<HotRecommend>>.Success(hots));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<HotRecommend>>> Create([FromBody] CreateHotRecommendRequest request)
    {
        var hot = new HotRecommend
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = request.Title,
            Alt = request.Alt,
            Target = request.Target,
            Pictures = JsonSerializer.Serialize(request.Pictures),
            Type = request.Type,
            Sort = request.Sort,
            Status = request.Status,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.HotRecommends.Add(hot);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<HotRecommend>.Success(hot, "添加成功"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<HotRecommend>>> Update(string id, [FromBody] CreateHotRecommendRequest request)
    {
        var existing = await _context.HotRecommends.FindAsync(id);
        if (existing == null)
        {
            return NotFound(ApiResponse<HotRecommend>.Error("推荐不存在"));
        }

        existing.Title = request.Title;
        existing.Alt = request.Alt;
        existing.Target = request.Target;
        existing.Pictures = JsonSerializer.Serialize(request.Pictures);
        existing.Type = request.Type;
        existing.Sort = request.Sort;
        existing.Status = request.Status;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<HotRecommend>.Success(existing, "修改成功"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id)
    {
        var hot = await _context.HotRecommends.FindAsync(id);
        if (hot == null)
        {
            return NotFound(ApiResponse.Error("推荐不存在"));
        }

        _context.HotRecommends.Remove(hot);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse.Success("删除成功"));
    }
}

public class CreateHotRecommendRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Alt { get; set; }
    public string? Target { get; set; }
    public List<string> Pictures { get; set; } = new();
    public string Type { get; set; } = string.Empty;
    public int Sort { get; set; }
    public int Status { get; set; }
}
