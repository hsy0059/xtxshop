using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Controllers.Admin;

[ApiController]
[Route("admin/[controller]")]
public class BannerController : ControllerBase
{
    private readonly AppDbContext _context;

    public BannerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<Banner>>>> GetList()
    {
        var banners = await _context.Banners
            .OrderBy(b => b.Sort)
            .ToListAsync();
        return Ok(ApiResponse<List<Banner>>.Success(banners));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Banner>>> Create([FromBody] Banner banner)
    {
        banner.Id = Guid.NewGuid().ToString("N");
        banner.CreatedAt = DateTime.Now;
        banner.UpdatedAt = DateTime.Now;
        _context.Banners.Add(banner);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Banner>.Success(banner, "添加成功"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Banner>>> Update(string id, [FromBody] Banner banner)
    {
        var existing = await _context.Banners.FindAsync(id);
        if (existing == null)
        {
            return NotFound(ApiResponse<Banner>.Error("轮播图不存在"));
        }

        existing.ImgUrl = banner.ImgUrl;
        existing.HrefUrl = banner.HrefUrl;
        existing.Type = banner.Type;
        existing.Sort = banner.Sort;
        existing.Status = banner.Status;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Banner>.Success(existing, "修改成功"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id)
    {
        var banner = await _context.Banners.FindAsync(id);
        if (banner == null)
        {
            return NotFound(ApiResponse.Error("轮播图不存在"));
        }

        _context.Banners.Remove(banner);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse.Success("删除成功"));
    }
}
