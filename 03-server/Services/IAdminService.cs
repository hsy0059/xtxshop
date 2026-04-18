using XtxServer.DTOs;

namespace XtxServer.Services;

public interface IAdminService
{
    Task<AdminLoginResult?> LoginAsync(AdminLoginRequest request);
    Task<AdminUserInfo?> GetAdminInfoAsync(string adminId);
    Task<DashboardStats> GetDashboardStatsAsync();
}
