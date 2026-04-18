using XtxServer.DTOs;

namespace XtxServer.Services;

public interface IGoodsService
{
    Task<GoodsResult?> GetGoodsByIdAsync(string id);
    Task<List<CategoryTopItem>> GetCategoryTopAsync();
    Task<List<GoodsItem>> GetSimilarProductsAsync(string id);
}
