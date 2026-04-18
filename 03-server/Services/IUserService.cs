using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Services;

public interface IUserService
{
    Task<LoginResult?> LoginAsync(LoginRequest request);
    Task<LoginResult?> SimpleLoginAsync(SimpleLoginRequest request);
    Task<ProfileDetail?> GetProfileAsync(string userId);
    Task<ProfileDetail?> UpdateProfileAsync(string userId, ProfileUpdateRequest request);
    Task<User?> GetUserByIdAsync(string userId);
}
