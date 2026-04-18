namespace XtxServer.DTOs;

public class CartAddRequest
{
    public string SkuId { get; set; } = string.Empty;
    public int Count { get; set; } = 1;
}

public class CartUpdateRequest
{
    public bool? Selected { get; set; }
    public int? Count { get; set; }
}

public class CartDeleteRequest
{
    public List<string> Ids { get; set; } = new();
}

public class CartSelectedRequest
{
    public bool Selected { get; set; }
}

public class CartItemDto
{
    public string Id { get; set; } = string.Empty;
    public string SkuId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Price { get; set; }
    public decimal NowPrice { get; set; }
    public int Stock { get; set; }
    public bool Selected { get; set; }
    public string AttrsText { get; set; } = string.Empty;
    public bool IsEffective { get; set; }
}
