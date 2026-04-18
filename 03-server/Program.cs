using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using XtxServer.Data;
using XtxServer.Middleware;
using XtxServer.Services;

var builder = WebApplication.CreateBuilder(args);

// 添加控制器和API文档
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 添加健康检查
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database");

// 配置数据库连接
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 0)),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});

// 配置Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    try
    {
        return ConnectionMultiplexer.Connect(redisConnection);
    }
    catch
    {
        // Redis连接失败不影响应用启动
        return ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false");
    }
});

// 注册服务
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IGoodsService, GoodsService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPayService, PayService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// 配置CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 开发环境配置
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用CORS
app.UseCors("AllowAll");

// 使用认证中间件
app.UseAuthMiddleware();

app.UseAuthorization();

// 映射控制器和健康检查端点
app.MapControllers();
app.MapHealthChecks("/health");

// 初始化数据库
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        await DbInitializer.InitializeAsync(context);
        Console.WriteLine("✅ 数据库初始化成功");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ 数据库初始化失败: {ex.Message}");
        Console.WriteLine("请检查:");
        Console.WriteLine("  1. MySQL服务是否已启动");
        Console.WriteLine("  2. 连接字符串是否正确");
        Console.WriteLine("  3. 数据库用户权限是否足够");
    }
}

app.Run();
