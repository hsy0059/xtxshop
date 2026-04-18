using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public UserService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResult?> LoginAsync(LoginRequest request)
    {
        var mobile = $"138{request.Code.Substring(0, 8)}";
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Mobile == mobile);

        if (user == null)
        {
            user = new User
            {
                Mobile = mobile,
                Account = $"user_{mobile[^4..]}",
                Nickname = $"用户{mobile[^4..]}",
                Avatar = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/avatar_1.jpg"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var token = _jwtService.GenerateToken(user.Id, user.Mobile);

        return new LoginResult
        {
            Id = user.Id,
            Mobile = user.Mobile,
            Token = token,
            Nickname = user.Nickname,
            Avatar = user.Avatar,
            Account = user.Account
        };
    }

    public async Task<LoginResult?> SimpleLoginAsync(SimpleLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Mobile == request.PhoneNumber);

        if (user == null)
        {
            user = new User
            {
                Mobile = request.PhoneNumber,
                Account = $"user_{request.PhoneNumber[^4..]}",
                Nickname = $"用户{request.PhoneNumber[^4..]}",
                Avatar = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/avatar_1.jpg"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var token = _jwtService.GenerateToken(user.Id, user.Mobile);

        return new LoginResult
        {
            Id = user.Id,
            Mobile = user.Mobile,
            Token = token,
            Nickname = user.Nickname,
            Avatar = user.Avatar,
            Account = user.Account
        };
    }

    public async Task<ProfileDetail?> GetProfileAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        return new ProfileDetail
        {
            Id = user.Id,
            Avatar = user.Avatar,
            Nickname = user.Nickname,
            Account = user.Account,
            Gender = user.Gender,
            Birthday = user.Birthday?.ToString("yyyy-MM-dd"),
            FullLocation = user.FullLocation ?? string.Empty,
            Profession = user.Profession
        };
    }

    public async Task<ProfileDetail?> UpdateProfileAsync(string userId, ProfileUpdateRequest request)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        user.Nickname = request.Nickname ?? user.Nickname;
        user.Gender = request.Gender ?? user.Gender;
        user.Profession = request.Profession ?? user.Profession;
        user.ProvinceCode = request.ProvinceCode ?? user.ProvinceCode;
        user.CityCode = request.CityCode ?? user.CityCode;
        user.CountyCode = request.CountyCode ?? user.CountyCode;

        if (!string.IsNullOrEmpty(request.Birthday) && DateTime.TryParse(request.Birthday, out var birthday))
        {
            user.Birthday = birthday;
        }

        if (!string.IsNullOrEmpty(user.ProvinceCode) && !string.IsNullOrEmpty(user.CityCode) && !string.IsNullOrEmpty(user.CountyCode))
        {
            user.FullLocation = $"{user.ProvinceCode} {user.CityCode} {user.CountyCode}";
        }

        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return await GetProfileAsync(userId);
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FindAsync(userId);
    }
}
