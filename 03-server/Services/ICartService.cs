using XtxServer.DTOs;

namespace XtxServer.Services;

public interface ICartService
{
    Task AddToCartAsync(string userId, CartAddRequest request);
    Task<List<CartItemDto>> GetCartAsync(string userId);
    Task UpdateCartItemAsync(string userId, string skuId, CartUpdateRequest request);
    Task DeleteCartItemsAsync(string userId, CartDeleteRequest request);
    Task UpdateCartSelectedAsync(string userId, CartSelectedRequest request);
}
