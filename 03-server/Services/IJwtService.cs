namespace XtxServer.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string mobile);
    string? ValidateToken(string token);
}
