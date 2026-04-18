using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;

namespace XtxServer.Services;

public class PayService : IPayService
{
    private readonly AppDbContext _context;

    public PayService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WxPayResult> GetWxPayParamsAsync(string userId, PayRequest request)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

        if (order == null) throw new Exception("订单不存在");

        return new WxPayResult
        {
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
            NonceStr = Guid.NewGuid().ToString("N")[..16],
            Package = $"prepay_id=wx{Guid.NewGuid().ToString("N")[..24]}",
            SignType = "RSA",
            PaySign = Guid.NewGuid().ToString("N")
        };
    }

    public async Task MockPayAsync(string userId, PayRequest request)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

        if (order == null) throw new Exception("订单不存在");

        order.OrderState = 2;
        order.PayTime = DateTime.Now;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}
