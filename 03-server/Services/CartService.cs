using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddToCartAsync(string userId, CartAddRequest request)
    {
        var sku = await _context.Skus
            .Include(s => s.Goods)
            .FirstOrDefaultAsync(s => s.Id == request.SkuId);

        if (sku == null) throw new Exception("SKU不存在");

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.SkuId == request.SkuId);

        if (existingItem != null)
        {
            existingItem.Count += request.Count;
            existingItem.UpdatedAt = DateTime.Now;
        }
        else
        {
            var cartItem = new CartItem
            {
                UserId = userId,
                SkuId = request.SkuId,
                GoodsId = sku.GoodsId,
                GoodsName = sku.Goods?.Name,
                Picture = sku.Picture ?? sku.Goods?.MainPicture,
                Count = request.Count,
                Price = sku.Price,
                NowPrice = sku.Price,
                Stock = sku.Inventory,
                Selected = true,
                IsEffective = sku.Inventory > 0 && sku.Goods?.Status == 1
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItemDto>> GetCartAsync(string userId)
    {
        var cartItems = await _context.CartItems
            .Include(c => c.Sku)
            .Include(c => c.Goods)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return cartItems.Select(c => new CartItemDto
        {
            Id = c.Id,
            SkuId = c.SkuId,
            Name = c.GoodsName ?? c.Goods?.Name ?? string.Empty,
            Picture = c.Picture ?? string.Empty,
            Count = c.Count,
            Price = c.Price,
            NowPrice = c.NowPrice,
            Stock = c.Stock,
            Selected = c.Selected,
            AttrsText = c.AttrsText ?? string.Empty,
            IsEffective = c.IsEffective && c.Stock > 0 && (c.Goods?.Status ?? 0) == 1
        }).ToList();
    }

    public async Task UpdateCartItemAsync(string userId, string skuId, CartUpdateRequest request)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.SkuId == skuId);

        if (cartItem == null) throw new Exception("购物车项不存在");

        if (request.Selected.HasValue)
        {
            cartItem.Selected = request.Selected.Value;
        }

        if (request.Count.HasValue)
        {
            cartItem.Count = request.Count.Value;
        }

        cartItem.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCartItemsAsync(string userId, CartDeleteRequest request)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId && request.Ids.Contains(c.SkuId))
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartSelectedAsync(string userId, CartSelectedRequest request)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();

        foreach (var item in cartItems)
        {
            item.Selected = request.Selected;
            item.UpdatedAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();
    }
}
