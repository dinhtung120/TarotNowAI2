/*
 * ===================================================================
 * FILE: DiagController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller CHẨN ĐOÁN (Diagnostics) - chỉ dùng trong MÔI TRƯỜNG PHÁT TRIỂN (Development).
 *   Cung cấp các công cụ để developer debug và setup hệ thống:
 *   1. Wipe: Xóa dữ liệu (hiện tại đã disabled)
 *   2. Seed Admin: Tự động tạo tài khoản Super Admin
 *   3. Stats: Xem thống kê dữ liệu MongoDB
 *
 * BẢO MẬT NHIỀU TẦNG:
 *   Tầng 1: [Authorize(Roles = "admin")] → chỉ admin mới gọi được
 *   Tầng 2: RejectIfNotDevelopment() → chỉ chạy trong môi trường Development
 *   Tầng 3: [ApiExplorerSettings(IgnoreApi = true)] → ẩn khỏi Swagger/OpenAPI
 *   
 *   Ngay cả khi hacker biết URL, họ cần:
 *   - Có JWT token admin (tầng 1)
 *   - Server phải đang chạy ở Development mode (tầng 2)
 *   Trong Production, tất cả endpoint đều trả 404 Not Found.
 *
 * CẢNH BÁO:
 *   KHÔNG BAO GIỜ để endpoint này hoạt động trên production server.
 *   Nó có quyền tạo admin, xóa dữ liệu - rất nguy hiểm nếu bị lạm dụng.
 * ===================================================================
 */

using Microsoft.AspNetCore.Authorization; // [Authorize] kiểm soát quyền
using Microsoft.AspNetCore.Mvc;           // API controller base

using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

/*
 * [ApiExplorerSettings(IgnoreApi = true)]:
 *   ẩn tất cả endpoint trong controller này khỏi Swagger UI.
 *   Swagger là trang web tự động sinh tài liệu API (thường ở /swagger).
 *   Ẩn đi vì đây là endpoint nội bộ, không muốn ai biết đến.
 */
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "admin")]
[ApiExplorerSettings(IgnoreApi = true)] // Ẩn khỏi Swagger - endpoint bí mật
public class DiagController : ControllerBase
{
    private readonly IDiagnosticsService _diagnosticsService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DiagController> _logger;

    public DiagController(
        IDiagnosticsService diagnosticsService,
        IWebHostEnvironment environment,
        ILogger<DiagController> logger)
    {
        _diagnosticsService = diagnosticsService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Guard method (hàm bảo vệ): kiểm tra môi trường hiện tại.
    /// Nếu KHÔNG phải Development → trả 404 Not Found (giả vờ endpoint không tồn tại).
    /// Nếu là Development → trả null (cho phép tiếp tục xử lý).
    /// 
    /// "Guard clause" (mệnh đề bảo vệ) là pattern thường dùng:
    ///   thay vì if-else lồng nhau, kiểm tra điều kiện sai → return sớm.
    /// </summary>
    private IActionResult? RejectIfNotDevelopment()
    {
        // IsDevelopment(): kiểm tra biến môi trường ASPNETCORE_ENVIRONMENT = "Development"
        if (_environment.IsDevelopment()) return null; // null = OK, tiếp tục
        return NotFound(); // Giả vờ endpoint không tồn tại trong Production
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Diag/wipe
    /// MỤC ĐÍCH: Xóa toàn bộ dữ liệu (database reset).
    /// HIỆN TRẠNG: Đã bị vô hiệu hóa (disabled) để tránh tai nạn.
    /// </summary>
    [HttpPost("wipe")]
    public IActionResult Wipe()
    {
        // Kiểm tra môi trường Development
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard; // Không phải Dev → 404

        // Trả notice: tính năng đã bị tắt
        return Ok(new { message = "Wipe endpoint is disabled by default." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Diag/seed-admin
    /// MỤC ĐÍCH: Tạo hoặc cập nhật tài khoản Super Admin.
    ///
    /// KHI NÀO DÙNG?
    ///   - Lần đầu setup hệ thống (database mới, chưa có admin nào)
    ///   - Quên mật khẩu admin (dùng endpoint này để reset)
    ///   - Chuyển quyền admin sang email khác
    ///
    /// CÁCH CẤU HÌNH:
    ///   Thêm vào appsettings.Development.json:
    ///   {
    ///     "Diagnostics": {
    ///       "SeedAdmin": {
    ///         "Email": "admin@tarot.com",
    ///         "Username": "superadmin",
    ///         "Password": "StrongP@ssw0rd123"
    ///       }
    ///     }
    ///   }
    /// </summary>
    [HttpPost("seed-admin")]
    public async Task<IActionResult> SeedAdmin()
    {
        // Guard: chỉ cho phép trong Development
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try 
        {
            var result = await _diagnosticsService.SeedAdminAsync(HttpContext.RequestAborted);
            if (result.Status == SeedAdminStatus.InvalidConfiguration)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new { 
                Message = result.Message, 
                Email = result.Email, 
                Username = result.Username
                // KHÔNG trả về password hay hash → bảo mật
            });
        }
        catch (Exception ex)
        {
            // Ghi log lỗi và trả 500
            _logger.LogError(ex, "Failed to seed admin account");
            return StatusCode(500, new { message = "Failed to seed admin account." });
        }
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Diag/stats
    /// MỤC ĐÍCH: Xem thống kê dữ liệu MongoDB để kiểm tra hệ thống hoạt động đúng.
    ///
    /// THÔNG TIN TRẢ VỀ:
    ///   - Tổng số reading sessions trong MongoDB
    ///   - Số sessions của một test user cụ thể
    ///   - 5 document mẫu (sample) để kiểm tra cấu trúc dữ liệu
    ///   
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        // Guard: chỉ cho phép trong Development
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try
        {
            var stats = await _diagnosticsService.GetStatsAsync(HttpContext.RequestAborted);

            // Trả về thống kê
            return Ok(new { 
                stats.TotalSessionsInMongo,
                stats.TestUserSessions,
                stats.SampleDataRaw
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch diagnostics stats");
            return StatusCode(500, new { message = "Failed to fetch diagnostics stats." });
        }
    }
}
