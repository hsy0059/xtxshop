using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AdminService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AdminLoginResult?> LoginAsync(AdminLoginRequest request)
    {
        var admin = await _context.AdminUsers
            .FirstOrDefaultAsync(a => a.Username == request.Username && a.Status == 1);

        if (admin == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.Password))
            return null;

        admin.LastLoginAt = DateTime.Now;
        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateToken(admin.Id, admin.Username);

        return new AdminLoginResult
        {
            Token = token,
            Id = admin.Id,
            Username = admin.Username,
            Nickname = admin.Nickname,
            Avatar = admin.Avatar
        };
    }

    public async Task<AdminUserInfo?> GetAdminInfoAsync(string adminId)
    {
        var admin = await _context.AdminUsers.FindAsync(adminId);
        if (admin == null) return null;

        return new AdminUserInfo
        {
            Id = admin.Id,
            Username = admin.Username,
            Nickname = admin.Nickname,
            Avatar = admin.Avatar,
            Email = admin.Email,
            Status = admin.Status,
            Role = admin.Role,
            CreatedAt = admin.CreatedAt,
            LastLoginAt = admin.LastLoginAt
        };
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalOrders = await _context.Orders.CountAsync();
        var totalGoods = await _context.Goods.CountAsync();
        var totalSales = await _context.Orders
            .Where(o => o.OrderState == 5)
            .SumAsync(o => o.PayMoney);

        var orderStats = new List<OrderStatsItem>();
        for (int i = 1; i <= 6; i++)
        {
            var count = await _context.Orders.CountAsync(o => o.OrderState == i);
            orderStats.Add(new OrderStatsItem
            {
                State = i,
                StateText = GetOrderStateText(i),
                Count = count
            });
        }

        var salesTrend = new List<SalesTrendItem>();
        for (int i = 6; i >= 0; i--)
        {
            var date = DateTime.Now.AddDays(-i);
            var amount = await _context.Orders
                .Where(o => o.OrderState == 5 && o.CreateTime.Date == date.Date)
                .SumAsync(o => o.PayMoney);
            salesTrend.Add(new SalesTrendItem
            {
                Date = date.ToString("MM-dd"),
                Amount = amount
            });
        }

        return new DashboardStats
        {
            TotalUsers = totalUsers,
            TotalOrders = totalOrders,
            TotalGoods = totalGoods,
            TotalSales = totalSales,
            OrderStats = orderStats,
            SalesTrend = salesTrend
        };
    }

    private string GetOrderStateText(int state)
    {
        return state switch
        {
            1 => "待付款",
            2 => "待发货",
            3 => "待收货",
            4 => "待评价",
            5 => "已完成",
            6 => "已取消",
            _ => "未知"
        };
    }
}
