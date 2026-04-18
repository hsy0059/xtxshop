namespace XtxServer.DTOs;

public class AdminLoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AdminLoginResult
{
    public string Token { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
}

public class AdminUserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public int Status { get; set; }
    public int Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class DashboardStats
{
    public int TotalUsers { get; set; }
    public int TotalOrders { get; set; }
    public int TotalGoods { get; set; }
    public decimal TotalSales { get; set; }
    public List<OrderStatsItem> OrderStats { get; set; } = new();
    public List<SalesTrendItem> SalesTrend { get; set; } = new();
}

public class OrderStatsItem
{
    public int State { get; set; }
    public string StateText { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class SalesTrendItem
{
    public string Date { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
