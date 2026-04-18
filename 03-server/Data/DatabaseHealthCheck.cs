using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MySqlConnector;

namespace XtxServer.Data;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly AppDbContext _context;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(AppDbContext context, ILogger<DatabaseHealthCheck> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // 测试数据库连接
            await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
            
            _logger.LogInformation("数据库连接正常");
            return HealthCheckResult.Healthy("数据库连接正常");
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "数据库连接失败: {Message}", ex.Message);
            return HealthCheckResult.Unhealthy($"数据库连接失败: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "健康检查发生错误: {Message}", ex.Message);
            return HealthCheckResult.Unhealthy($"健康检查错误: {ex.Message}");
        }
    }
}
