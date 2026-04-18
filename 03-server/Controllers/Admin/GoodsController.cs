using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Controllers.Admin;

[ApiController]
[Route("admin/[controller]")]
public class GoodsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GoodsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? keyword = null,
        [FromQuery] string? categoryId = null)
    {
        var query = _context.Goods.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(g => g.Name.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(categoryId))
        {
            query = query.Where(g => g.CategoryId == categoryId);
        }

        var total = await query.CountAsync();
        var list = await query
            .Include(g => g.Category)
            .OrderByDescending(g => g.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new
            {
                g.Id,
                g.Name,
                g.Desc,
                g.Price,
                g.OldPrice,
                g.MainPicture,
                g.Inventory,
                g.SalesCount,
                g.Status,
                g.CategoryId,
                CategoryName = g.Category != null ? g.Category.Name : null,
                g.CreatedAt
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Success(new { list, total }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Goods>>> GetById(string id)
    {
        var goods = await _context.Goods.FindAsync(id);
        if (goods == null)
        {
            return NotFound(ApiResponse<Goods>.Error("商品不存在"));
        }
        return Ok(ApiResponse<Goods>.Success(goods));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Goods>>> Create([FromBody] Goods goods)
    {
        goods.Id = Guid.NewGuid().ToString("N");
        goods.CreatedAt = DateTime.Now;
        goods.UpdatedAt = DateTime.Now;
        _context.Goods.Add(goods);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Goods>.Success(goods, "添加成功"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Goods>>> Update(string id, [FromBody] Goods goods)
    {
        var existing = await _context.Goods.FindAsync(id);
        if (existing == null)
        {
            return NotFound(ApiResponse<Goods>.Error("商品不存在"));
        }

        existing.Name = goods.Name;
        existing.Desc = goods.Desc;
        existing.Price = goods.Price;
        existing.OldPrice = goods.OldPrice;
        existing.Inventory = goods.Inventory;
        existing.CategoryId = goods.CategoryId;
        existing.MainPicture = goods.MainPicture;
        existing.Status = goods.Status;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Goods>.Success(existing, "修改成功"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id)
    {
        var goods = await _context.Goods.FindAsync(id);
        if (goods == null)
        {
            return NotFound(ApiResponse.Error("商品不存在"));
        }

        _context.Goods.Remove(goods);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse.Success("删除成功"));
    }
}

[ApiController]
[Route("admin/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<Category>>>> GetList()
    {
        var categories = await _context.Categories
            .Where(c => c.ParentId == null)
            .Include(c => c.Children)
            .OrderBy(c => c.Sort)
            .ToListAsync();
        return Ok(ApiResponse<List<Category>>.Success(categories));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Category>>> Create([FromBody] Category category)
    {
        category.Id = Guid.NewGuid().ToString("N");
        category.CreatedAt = DateTime.Now;
        category.UpdatedAt = DateTime.Now;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Category>.Success(category, "添加成功"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Category>>> Update(string id, [FromBody] Category category)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null)
        {
            return NotFound(ApiResponse<Category>.Error("分类不存在"));
        }

        existing.Name = category.Name;
        existing.ParentId = category.ParentId;
        existing.Icon = category.Icon;
        existing.Picture = category.Picture;
        existing.Sort = category.Sort;
        existing.Level = category.Level;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Category>.Success(existing, "修改成功"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound(ApiResponse.Error("分类不存在"));
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse.Success("删除成功"));
    }
}
