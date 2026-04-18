using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;

namespace XtxServer.Controllers.Admin;

[ApiController]
[Route("admin/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? orderState = null,
        [FromQuery] string? keyword = null)
    {
        var query = _context.Orders
            .Include(o => o.Items)
            .AsQueryable();

        if (orderState.HasValue)
        {
            query = query.Where(o => o.OrderState == orderState.Value);
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(o =>
                o.Id.Contains(keyword) ||
                (o.ReceiverContact != null && o.ReceiverContact.Contains(keyword)) ||
                (o.ReceiverMobile != null && o.ReceiverMobile.Contains(keyword)));
        }

        var total = await query.CountAsync();
        var list = await query
            .OrderByDescending(o => o.CreateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new
            {
                o.Id,
                o.OrderState,
                o.ReceiverContact,
                o.ReceiverMobile,
                o.ReceiverAddress,
                o.TotalMoney,
                o.PostFee,
                o.PayMoney,
                o.CreateTime,
                o.PayTime,
                o.ShipTime,
                o.ReceiveTime,
                Items = o.Items.Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Image,
                    i.Quantity,
                    i.CurPrice,
                    i.AttrsText
                })
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Success(new { list, total }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(string id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound(ApiResponse<object>.Error("订单不存在"));
        }

        return Ok(ApiResponse<object>.Success(order));
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<ApiResponse>> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound(ApiResponse.Error("订单不存在"));
        }

        order.OrderState = request.OrderState;
        order.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse.Success("修改成功"));
    }

    [HttpPut("{id}/ship")]
    public async Task<ActionResult<ApiResponse>> Ship(string id, [FromBody] ShipRequest request)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound(ApiResponse.Error("订单不存在"));
        }

        order.OrderState = 3;
        order.ShipTime = DateTime.Now;
        order.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse.Success("发货成功"));
    }
}

public class UpdateStatusRequest
{
    public int OrderState { get; set; }
    public string? Remark { get; set; }
}

public class ShipRequest
{
    public string LogisticsCompany { get; set; } = string.Empty;
    public string LogisticsNo { get; set; } = string.Empty;
}
