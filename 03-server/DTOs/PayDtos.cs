namespace XtxServer.DTOs;

public class PayRequest
{
    public string OrderId { get; set; } = string.Empty;
}

public class WxPayResult
{
    public string TimeStamp { get; set; } = string.Empty;
    public string NonceStr { get; set; } = string.Empty;
    public string Package { get; set; } = string.Empty;
    public string SignType { get; set; } = string.Empty;
    public string PaySign { get; set; } = string.Empty;
}
