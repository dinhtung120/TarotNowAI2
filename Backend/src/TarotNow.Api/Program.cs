/*
 * ===================================================================
 * FILE: Program.cs
 * NAMESPACE: (không có namespace - đây là top-level file)
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   FILE KHỞI ĐỘNG CHÍNH (ENTRY POINT) của toàn bộ ứng dụng Backend.
 *   Giống như "hàm main()" trong các ngôn ngữ khác.
 *
 *   File này thực hiện 2 việc chính:
 *   1. CẤU HÌNH SERVICES: Đăng ký tất cả dependency injection, middleware, plugin
 *   2. CẤU HÌNH PIPELINE: Sắp xếp thứ tự xử lý HTTP request
 *
 * TOP-LEVEL STATEMENTS (CÚ PHÁP MỚI C# 9+):
 *   Không cần viết class Program { static void Main() { ... } }
 *   Chỉ cần viết code trực tiếp → C# tự hiểu đây là entry point.
 *   Làm code gọn hơn, dễ đọc hơn.
 *
 * THỨ TỰ PIPELINE RẤT QUAN TRỌNG:
 *   Mỗi request HTTP đi qua các middleware theo THỨ TỰ từ trên xuống:
 *   Request → ExceptionHandler → CORS → Authentication → Authorization → Controller
 *   Response ← ExceptionHandler ← CORS ← Authentication ← Authorization ← Controller
 *   Nếu đặt sai thứ tự → ứng dụng hoạt động sai hoặc lỗ hổng bảo mật.
 * ===================================================================
 */

// Import các module cần thiết
using TarotNow.Application;   // Extension method đăng ký Application services
using TarotNow.Infrastructure; // Extension method đăng ký Infrastructure services
using TarotNow.Api.Middlewares; // GlobalExceptionHandler
using TarotNow.Api.Hubs;       // ChatHub (SignalR)

/*
 * WebApplication.CreateBuilder(args):
 * Tạo builder để cấu hình ứng dụng web.
 * "args" là command-line arguments (tham số dòng lệnh khi chạy ứng dụng).
 * Builder tự động đọc: appsettings.json, biến môi trường, user secrets.
 */
var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// PHẦN 1: CẤU HÌNH SERVICES (Dependency Injection Container)
// =========================================================================
// Đăng ký tất cả services (dịch vụ) vào DI container.
// Sau khi đăng ký, mọi class đều có thể "xin" service qua constructor.

/*
 * AddControllers(): Đăng ký tất cả API controllers.
 * Tự động scan tất cả class kế thừa ControllerBase trong project.
 * Ví dụ: AuthController, WalletController,... đều được đăng ký ở đây.
 */
builder.Services.AddControllers();

/*
 * AddOpenApi(): Đăng ký OpenAPI (Swagger) specification.
 * Tự động sinh tài liệu API từ code (endpoint URL, tham số, response type).
 * Developer dùng Swagger UI để test API mà không cần Postman.
 * Chuẩn .NET 9 (thay thế Swashbuckle từ .NET 8 trở xuống).
 */
builder.Services.AddOpenApi();

/*
 * AddExceptionHandler<GlobalExceptionHandler>():
 * Đăng ký bộ xử lý lỗi toàn cục.
 * Mọi exception không được catch → GlobalExceptionHandler xử lý.
 * 
 * AddProblemDetails():
 * Bật hỗ trợ RFC 7807 ProblemDetails cho toàn ứng dụng.
 * Đảm bảo mọi lỗi đều trả format chuẩn JSON thống nhất.
 */
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // RFC 7807 support

/*
 * AddApplicationServices(): Extension method tự viết (ở TarotNow.Application).
 * Đăng ký: MediatR, AutoMapper, FluentValidation, và các service Application layer.
 * Tách riêng để mỗi layer quản lý đăng ký DI của mình (Clean Architecture).
 * 
 * AddInfrastructureServices(): Extension method tự viết (ở TarotNow.Infrastructure).
 * Đăng ký: EF Core (PostgreSQL), MongoDB, Redis, JWT Authentication,
 *   IPasswordHasher, IEmailService, IAiProvider, v.v.
 * Nhận builder.Configuration để đọc connection string, API keys.
 */
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

/*
 * AddSignalR(): Đăng ký SignalR cho chat realtime.
 * SignalR tự quản lý:
 * - WebSocket connections (kết nối 2 chiều)
 * - Fallback transport (SSE, Long Polling nếu WebSocket không khả dụng)
 * - Memory management cho connections và groups
 */
builder.Services.AddSignalR();

/*
 * CẤU HÌNH CORS (Cross-Origin Resource Sharing):
 * 
 * CORS LÀ GÌ?
 *   Trình duyệt mặc định CHẶN request từ domain khác (bảo mật).
 *   Ví dụ: Frontend ở https://tarotnow.ai gọi API ở https://api.tarotnow.ai
 *   → Trình duyệt chặn vì 2 domain khác nhau.
 *   CORS cho phép server nói "OK, tôi cho phép domain X gọi API của tôi".
 * 
 * Cấu hình trong appsettings.json:
 * {
 *   "Cors": {
 *     "AllowedOrigins": ["https://tarotnow.ai", "http://localhost:3000"]
 *   }
 * }
 */
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?
    .Where(origin => !string.IsNullOrWhiteSpace(origin)) // Bỏ qua origin rỗng
    .Select(origin => origin.Trim())                      // Xóa khoảng trắng thừa
    .Distinct(StringComparer.OrdinalIgnoreCase)            // Loại bỏ trùng lặp
    .ToArray() ?? Array.Empty<string>();                   // Nếu null → mảng rỗng

// BẮT BUỘC phải có ít nhất 1 origin → tránh quên cấu hình khi deploy
if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Missing CORS configuration: Cors:AllowedOrigins must contain at least one origin.");
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Thay vì WithOrigins, cho phép mọi client domain kể cả di động
              .AllowAnyHeader()            
              .AllowAnyMethod()            
              .AllowCredentials();         
    });
});

// =========================================================================
// PHẦN 2: CẤU HÌNH HTTP REQUEST PIPELINE (Middleware Pipeline)
// =========================================================================
// Sắp xếp thứ tự middleware xử lý mỗi HTTP request.
// THỨ TỰ RẤT QUAN TRỌNG — đặt sai sẽ gây lỗi hoặc lỗ hổng bảo mật.

var app = builder.Build();

/*
 * UseExceptionHandler(): Kích hoạt GlobalExceptionHandler.
 * PHẢI ĐẶT ĐẦU TIÊN: để bắt mọi exception từ tất cả middleware phía sau.
 * Nếu đặt sau UseAuthentication → lỗi authentication không được xử lý đúng.
 */
app.UseExceptionHandler();

/*
 * MapOpenApi(): Bật endpoint OpenAPI (Swagger) chỉ trong Development.
 * KHÔNG bật trong Production vì:
 * 1. Lộ cấu trúc API cho hacker
 * 2. Tốn tài nguyên server
 */
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

/*
 * UseHttpsRedirection(): Tự động chuyển HTTP → HTTPS.
 * Chỉ bật ngoài Development vì dev thường dùng HTTP (http://localhost:5000).
 * Production BẮT BUỘC HTTPS để mã hóa traffic.
 */
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

/*
 * UseCors(): Bật CORS middleware.
 * PHẢI đặt TRƯỚC UseAuthentication() và UseAuthorization().
 * Nếu đặt sau → trình duyệt bị chặn preflight request (OPTIONS)
 * trước khi authentication middleware kịp xử lý.
 */
app.UseCors();

/*
 * UseAuthentication(): Xác thực JWT token.
 * Đọc header "Authorization: Bearer {token}" → parse JWT → gán User identity.
 * 
 * UseAuthorization(): Phân quyền dựa trên identity.
 * Kiểm tra [Authorize], [Authorize(Roles = "admin")], v.v.
 * 
 * THỨ TỰ BẮT BUỘC: Authentication TRƯỚC Authorization.
 * Phải biết "Ai" (authentication) trước khi kiểm tra "Có quyền không" (authorization).
 */
app.UseAuthentication();
app.UseAuthorization();

/*
 * MapControllers(): Map tất cả controller endpoints vào pipeline.
 * Ví dụ: [HttpGet("balance")] trên WalletController
 * → tạo route GET /api/v1/Wallet/balance tự động.
 */
app.MapControllers();

/*
 * MapHub<ChatHub>("/api/v1/chat"):
 * Map SignalR ChatHub tại URL /api/v1/chat
 * 
 * Client kết nối:
 *   const conn = new HubConnectionBuilder()
 *     .withUrl("https://api.tarotnow.ai/api/v1/chat?access_token=xxx")
 *     .build();
 *   await conn.start();
 * 
 * JWT query string auth đã được cấu hình trong Infrastructure DependencyInjection.cs
 * (OnMessageReceived event handler đọc token từ query string).
 */
app.MapHub<ChatHub>("/api/v1/chat");

/*
 * app.Run(): Khởi động server và bắt đầu lắng nghe HTTP request.
 * Đây là dòng cuối cùng — sau dòng này, server chạy vô hạn
 * cho đến khi bị kill (Ctrl+C, docker stop, hoặc crash).
 */
app.Run();

/*
 * public partial class Program { }
 * 
 * TẠI SAO CẦN DÒNG NÀY?
 *   Hỗ trợ WebApplicationFactory cho Integration Tests.
 *   Trong test, WebApplicationFactory<Program> cần truy cập class Program.
 *   "partial" cho phép class Program được mở rộng ở file khác.
 *   Không có dòng này → integration test không compile được.
 */
public partial class Program { }
