using System.Security.Claims;
using XtxServer.Services;

namespace XtxServer.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        if (path.StartsWith("/login") ||
            path.StartsWith("/home") && !path.Contains("/member") ||
            path.StartsWith("/goods") ||
            path.StartsWith("/category") ||
            path.StartsWith("/admin/auth/login") ||
            path.StartsWith("/swagger"))
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { msg = "未登录或登录已过期", code = 401 });
            return;
        }

        var token = authHeader.Substring(7);
        var userId = jwtService.ValidateToken(token);

        if (string.IsNullOrEmpty(userId))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { msg = "Token无效或已过期", code = 401 });
            return;
        }

        context.Items["UserId"] = userId;
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "Bearer"));

        await _next(context);
    }
}

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}
