namespace XtxServer.DTOs;

public class GoodsResult
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public Details Details { get; set; } = new();
    public List<string> MainPictures { get; set; } = new();
    public List<GoodsItem> SimilarProducts { get; set; } = new();
    public List<SkuItem> Skus { get; set; } = new();
    public List<SpecItem> Specs { get; set; } = new();
    public List<AddressItem> UserAddresses { get; set; } = new();
}

public class GoodsItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Picture { get; set; } = string.Empty;
}

public class Details
{
    public List<DetailsPropertyItem> Properties { get; set; } = new();
    public List<string> Pictures { get; set; } = new();
}

public class DetailsPropertyItem
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class SkuItem
{
    public string Id { get; set; } = string.Empty;
    public int Inventory { get; set; }
    public decimal OldPrice { get; set; }
    public string? Picture { get; set; }
    public decimal Price { get; set; }
    public string SkuCode { get; set; } = string.Empty;
    public List<SkuSpecItem> Specs { get; set; } = new();
}

public class SkuSpecItem
{
    public string Name { get; set; } = string.Empty;
    public string ValueName { get; set; } = string.Empty;
}

public class SpecItem
{
    public string Name { get; set; } = string.Empty;
    public List<SpecValueItem> Values { get; set; } = new();
}

public class SpecValueItem
{
    public bool Available { get; set; }
    public string Desc { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Picture { get; set; }
}

public class CategoryTopItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public List<CategoryChildItem> Children { get; set; } = new();
    public List<GoodsItem> Goods { get; set; } = new();
}

public class CategoryChildItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
}
