using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("wxMin")]
    public async Task<ActionResult<ApiResponse<LoginResult>>> WxMinLogin([FromBody] LoginRequest request)
    {
        var result = await _userService.LoginAsync(request);
        if (result == null)
        {
            return BadRequest(ApiResponse<LoginResult>.Error("登录失败"));
        }
        return Ok(ApiResponse<LoginResult>.Success(result, "登录成功"));
    }

    [HttpPost("wxMin/simple")]
    public async Task<ActionResult<ApiResponse<LoginResult>>> WxMinSimpleLogin([FromBody] SimpleLoginRequest request)
    {
        var result = await _userService.SimpleLoginAsync(request);
        if (result == null)
        {
            return BadRequest(ApiResponse<LoginResult>.Error("登录失败"));
        }
        return Ok(ApiResponse<LoginResult>.Success(result, "登录成功"));
    }
}
