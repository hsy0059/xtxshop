using XtxServer.DTOs;

namespace XtxServer.Services;

public interface IPayService
{
    Task<WxPayResult> GetWxPayParamsAsync(string userId, PayRequest request);
    Task MockPayAsync(string userId, PayRequest request);
}
