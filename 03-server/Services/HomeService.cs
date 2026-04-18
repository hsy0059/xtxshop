using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;

namespace XtxServer.Services;

public class HomeService : IHomeService
{
    private readonly AppDbContext _context;

    public HomeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BannerItem>> GetBannersAsync(int distributionSite = 1)
    {
        return await _context.Banners
            .Where(b => b.Type == distributionSite && b.Status == 1)
            .OrderBy(b => b.Sort)
            .Select(b => new BannerItem
            {
                Id = b.Id,
                HrefUrl = b.HrefUrl ?? string.Empty,
                ImgUrl = b.ImgUrl,
                Type = b.Type
            })
            .ToListAsync();
    }

    public async Task<List<CategoryItem>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.Level == 1 && c.ParentId == null)
            .OrderBy(c => c.Sort)
            .Take(10)
            .Select(c => new CategoryItem
            {
                Id = c.Id,
                Icon = c.Icon ?? string.Empty,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<List<HotItem>> GetHotRecommendsAsync()
    {
        var hots = await _context.HotRecommends
            .Where(h => h.Status == 1)
            .OrderBy(h => h.Sort)
            .ToListAsync();

        return hots.Select(h => new HotItem
        {
            Id = h.Id,
            Alt = h.Alt ?? string.Empty,
            Pictures = h.Pictures == null
                ? new List<string>()
                : h.Pictures.Split(',').ToList(),
            Target = h.Target ?? string.Empty,
            Title = h.Title,
            Type = h.Type
        }).ToList();
    }

    public async Task<PageResult<GuessItem>> GetGuessLikeAsync(PageParams pageParams)
    {
        var query = _context.Goods
            .Where(g => g.Status == 1)
            .OrderByDescending(g => g.SalesCount);

        var total = await query.CountAsync();
        var items = await query
            .Skip((pageParams.Page - 1) * pageParams.PageSize)
            .Take(pageParams.PageSize)
            .Select(g => new GuessItem
            {
                Id = g.Id,
                Name = g.Name,
                Desc = g.Desc ?? string.Empty,
                Price = g.Price,
                Picture = g.MainPicture ?? string.Empty
            })
            .ToListAsync();

        return new PageResult<GuessItem>
        {
            Page = pageParams.Page,
            PageSize = pageParams.PageSize,
            Total = total,
            Pages = (int)Math.Ceiling((double)total / pageParams.PageSize),
            Items = items
        };
    }
}
