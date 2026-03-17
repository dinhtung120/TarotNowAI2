using TarotNow.Application;
using TarotNow.Infrastructure;
using TarotNow.Api.Middlewares;
using TarotNow.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. SERVICES CONFIGURATION
// =========================================================================

// Thêm Controllers
builder.Services.AddControllers();

// Cấu hình OpenAPI (chuẩn .NET 9)
builder.Services.AddOpenApi();

// Cấu hình Global Exception Handler (ProblemDetails)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // RFC 7807 support

// Thêm dependency injection cho Application và Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Phase 2.2: Đăng ký SignalR cho Chat realtime
// SignalR tự quản lý WebSocket connections + fallback.
builder.Services.AddSignalR();

// Cấu hình CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Hỗ trợ Authentication Cookies
    });
});

// =========================================================================
// 2. HTTP REQUEST PIPELINE
// =========================================================================

var app = builder.Build();

// Sử dụng Global Exception Handler middleware
app.UseExceptionHandler();

// Chỉ mở OpenAPI ở môi trường Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirect HTTP sang HTTPS (có thể tắt ở local dev nếu cần)
// app.UseHttpsRedirection(); 

// Phục vụ statics files (nếu có)
// app.UseStaticFiles();

// Bật CORS
app.UseCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map routes cho Controllers
// Map routes cho Controllers
app.MapControllers();

// Phase 2.2: Map SignalR ChatHub tại /api/v1/chat
// Client kết nối: new HubConnectionBuilder().withUrl("/api/v1/chat?access_token=xxx")
// JWT query string auth đã cấu hình trong DependencyInjection.cs
app.MapHub<ChatHub>("/api/v1/chat");

app.Run();

// Hỗ trợ Integration Test bằng WebApplicationFactory
public partial class Program { }
