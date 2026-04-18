using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("[controller]")]
public class PayController : ControllerBase
{
    private readonly IPayService _payService;

    public PayController(IPayService payService)
    {
        _payService = payService;
    }

    private string GetUserId()
    {
        return HttpContext.Items["UserId"]?.ToString() ?? string.Empty;
    }

    [HttpGet("wxPay/miniPay")]
    public async Task<ActionResult<ApiResponse<WxPayResult>>> WxPayMiniPay([FromQuery] PayRequest request)
    {
        var userId = GetUserId();
        var result = await _payService.GetWxPayParamsAsync(userId, request);
        return Ok(ApiResponse<WxPayResult>.Success(result));
    }

    [HttpGet("mock")]
    public async Task<ActionResult<ApiResponse>> MockPay([FromQuery] PayRequest request)
    {
        var userId = GetUserId();
        await _payService.MockPayAsync(userId, request);
        return Ok(ApiResponse.Success("支付成功"));
    }
}
