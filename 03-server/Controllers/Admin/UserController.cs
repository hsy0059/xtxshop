using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;

namespace XtxServer.Controllers.Admin;

[ApiController]
[Route("admin/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? keyword = null)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(u =>
                u.Account.Contains(keyword) ||
                u.Mobile.Contains(keyword) ||
                (u.Nickname != null && u.Nickname.Contains(keyword)));
        }

        var total = await query.CountAsync();
        var list = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.Account,
                u.Nickname,
                u.Avatar,
                u.Mobile,
                u.Gender,
                u.Birthday,
                u.FullLocation,
                u.Profession,
                u.CreatedAt
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Success(new { list, total }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<object>.Error("用户不存在"));
        }

        return Ok(ApiResponse<object>.Success(user));
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<ApiResponse>> UpdateStatus(string id, [FromBody] UpdateUserStatusRequest request)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse.Error("用户不存在"));
        }

        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse.Success("修改成功"));
    }
}

public class UpdateUserStatusRequest
{
    public int Status { get; set; }
}
