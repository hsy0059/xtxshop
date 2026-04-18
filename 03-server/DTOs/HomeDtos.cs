namespace XtxServer.DTOs;

public class BannerItem
{
    public string Id { get; set; } = string.Empty;
    public string HrefUrl { get; set; } = string.Empty;
    public string ImgUrl { get; set; } = string.Empty;
    public int Type { get; set; }
}

public class CategoryItem
{
    public string Id { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class HotItem
{
    public string Id { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
    public List<string> Pictures { get; set; } = new();
    public string Target { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class GuessItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Picture { get; set; } = string.Empty;
}

public class PageParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PageResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int Pages { get; set; }
    public List<T> Items { get; set; } = new();
}
