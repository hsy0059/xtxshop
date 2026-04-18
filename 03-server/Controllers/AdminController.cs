using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("admin/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AuthController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AdminLoginResult>>> Login([FromBody] AdminLoginRequest request)
    {
        var result = await _adminService.LoginAsync(request);
        if (result == null)
        {
            return BadRequest(ApiResponse<AdminLoginResult>.Error("用户名或密码错误"));
        }
        return Ok(ApiResponse<AdminLoginResult>.Success(result, "登录成功"));
    }

    [HttpGet("info")]
    public async Task<ActionResult<ApiResponse<AdminUserInfo>>> GetInfo()
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(ApiResponse<AdminUserInfo>.Error("未登录"));
        }

        var result = await _adminService.GetAdminInfoAsync(userId);
        if (result == null)
        {
            return NotFound(ApiResponse<AdminUserInfo>.Error("用户不存在"));
        }
        return Ok(ApiResponse<AdminUserInfo>.Success(result));
    }
}

[ApiController]
[Route("admin/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IAdminService _adminService;

    public DashboardController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<DashboardStats>>> GetStats()
    {
        var result = await _adminService.GetDashboardStatsAsync();
        return Ok(ApiResponse<DashboardStats>.Success(result));
    }
}
