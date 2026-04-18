using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using XtxServer.Data;
using XtxServer.DTOs;

namespace XtxServer.Services;

public class GoodsService : IGoodsService
{
    private readonly AppDbContext _context;

    public GoodsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GoodsResult?> GetGoodsByIdAsync(string id)
    {
        var goods = await _context.Goods
            .Include(g => g.Skus)
            .Include(g => g.Specs)
            .FirstOrDefaultAsync(g => g.Id == id && g.Status == 1);

        if (goods == null) return null;

        var result = new GoodsResult
        {
            Id = goods.Id,
            Name = goods.Name,
            Desc = goods.Desc ?? string.Empty,
            Price = goods.Price,
            OldPrice = goods.OldPrice,
            MainPictures = string.IsNullOrEmpty(goods.Pictures)
                ? new List<string>()
                : goods.Pictures.Split(',').ToList(),
            SimilarProducts = await GetSimilarProductsAsync(id),
            Skus = goods.Skus.Select(s => new SkuItem
            {
                Id = s.Id,
                Inventory = s.Inventory,
                OldPrice = s.OldPrice,
                Picture = s.Picture,
                Price = s.Price,
                SkuCode = s.SkuCode,
                Specs = string.IsNullOrEmpty(s.Specs)
                    ? new List<SkuSpecItem>()
                    : JsonSerializer.Deserialize<List<SkuSpecItem>>(s.Specs) ?? new List<SkuSpecItem>()
            }).ToList(),
            Specs = goods.Specs.Select(s => new SpecItem
            {
                Name = s.Name,
                Values = string.IsNullOrEmpty(s.Values)
                    ? new List<SpecValueItem>()
                    : JsonSerializer.Deserialize<List<SpecValueItem>>(s.Values) ?? new List<SpecValueItem>()
            }).ToList(),
            Details = new Details
            {
                Properties = string.IsNullOrEmpty(goods.Properties)
                    ? new List<DetailsPropertyItem>()
                    : JsonSerializer.Deserialize<List<DetailsPropertyItem>>(goods.Properties) ?? new List<DetailsPropertyItem>(),
                Pictures = string.IsNullOrEmpty(goods.Details)
                    ? new List<string>()
                    : goods.Details.Split(',').ToList()
            },
            UserAddresses = new List<AddressItem>()
        };

        return result;
    }

    public async Task<List<CategoryTopItem>> GetCategoryTopAsync()
    {
        var categories = await _context.Categories
            .Where(c => c.Level == 1 && c.ParentId == null)
            .Include(c => c.Children)
            .OrderBy(c => c.Sort)
            .ToListAsync();

        var result = new List<CategoryTopItem>();

        foreach (var category in categories)
        {
            var goods = await _context.Goods
                .Where(g => g.CategoryId == category.Id && g.Status == 1)
                .OrderByDescending(g => g.SalesCount)
                .Take(9)
                .Select(g => new GoodsItem
                {
                    Id = g.Id,
                    Name = g.Name,
                    Desc = g.Desc ?? string.Empty,
                    Price = g.Price,
                    Picture = g.MainPicture ?? string.Empty
                })
                .ToListAsync();

            result.Add(new CategoryTopItem
            {
                Id = category.Id,
                Name = category.Name,
                Picture = category.Picture ?? string.Empty,
                Children = category.Children.Select(c => new CategoryChildItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    Picture = c.Picture ?? string.Empty
                }).ToList(),
                Goods = goods
            });
        }

        return result;
    }

    public async Task<List<GoodsItem>> GetSimilarProductsAsync(string id)
    {
        var goods = await _context.Goods.FindAsync(id);
        if (goods == null) return new List<GoodsItem>();

        return await _context.Goods
            .Where(g => g.CategoryId == goods.CategoryId && g.Id != id && g.Status == 1)
            .OrderByDescending(g => g.SalesCount)
            .Take(5)
            .Select(g => new GoodsItem
            {
                Id = g.Id,
                Name = g.Name,
                Desc = g.Desc ?? string.Empty,
                Price = g.Price,
                Picture = g.MainPicture ?? string.Empty
            })
            .ToListAsync();
    }
}
