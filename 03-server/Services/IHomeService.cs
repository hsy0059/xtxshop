using XtxServer.DTOs;

namespace XtxServer.Services;

public interface IHomeService
{
    Task<List<BannerItem>> GetBannersAsync(int distributionSite = 1);
    Task<List<CategoryItem>> GetCategoriesAsync();
    Task<List<HotItem>> GetHotRecommendsAsync();
    Task<PageResult<GuessItem>> GetGuessLikeAsync(PageParams pageParams);
}
