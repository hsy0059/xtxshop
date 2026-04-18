namespace XtxServer.DTOs;

public class OrderPreResult
{
    public List<OrderPreGoods> Goods { get; set; } = new();
    public OrderSummary Summary { get; set; } = new();
    public List<AddressItem> UserAddresses { get; set; } = new();
}

public class OrderPreGoods
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public int Count { get; set; }
    public string SkuId { get; set; } = string.Empty;
    public string AttrsText { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string PayPrice { get; set; } = string.Empty;
    public string TotalPrice { get; set; } = string.Empty;
    public string TotalPayPrice { get; set; } = string.Empty;
}

public class OrderSummary
{
    public decimal TotalPrice { get; set; }
    public decimal PostFee { get; set; }
    public decimal TotalPayPrice { get; set; }
}

public class OrderCreateRequest
{
    public string AddressId { get; set; } = string.Empty;
    public int DeliveryTimeType { get; set; } = 1;
    public string BuyerMessage { get; set; } = string.Empty;
    public List<OrderGoodsRequest> Goods { get; set; } = new();
    public int PayChannel { get; set; } = 2;
    public int PayType { get; set; } = 1;
}

public class OrderGoodsRequest
{
    public int Count { get; set; }
    public string SkuId { get; set; } = string.Empty;
}

public class OrderCreateResult
{
    public string Id { get; set; } = string.Empty;
}

public class OrderResult
{
    public string Id { get; set; } = string.Empty;
    public int OrderState { get; set; }
    public int Countdown { get; set; }
    public List<OrderSkuItem> Skus { get; set; } = new();
    public string ReceiverContact { get; set; } = string.Empty;
    public string ReceiverMobile { get; set; } = string.Empty;
    public string ReceiverAddress { get; set; } = string.Empty;
    public string CreateTime { get; set; } = string.Empty;
    public decimal TotalMoney { get; set; }
    public decimal PostFee { get; set; }
    public decimal PayMoney { get; set; }
}

public class OrderSkuItem
{
    public string Id { get; set; } = string.Empty;
    public string SpuId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AttrsText { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal CurPrice { get; set; }
    public string Image { get; set; } = string.Empty;
}

public class OrderListParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int OrderState { get; set; } = 0;
}

public class OrderListResult
{
    public int Counts { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public int Page { get; set; }
    public int Pages { get; set; }
    public int PageSize { get; set; }
}

public class OrderItemDto : OrderResult
{
    public int TotalNum { get; set; }
}

public class OrderCancelRequest
{
    public string CancelReason { get; set; } = string.Empty;
}

public class OrderLogisticResult
{
    public LogisticCompany Company { get; set; } = new();
    public int Count { get; set; }
    public List<LogisticItem> List { get; set; } = new();
}

public class LogisticCompany
{
    public string Name { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
}

public class LogisticItem
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
}

public class OrderDeleteRequest
{
    public List<string> Ids { get; set; } = new();
}

public class OrderPreNowRequest
{
    public string SkuId { get; set; } = string.Empty;
    public string Count { get; set; } = string.Empty;
    public string? AddressId { get; set; }
}
