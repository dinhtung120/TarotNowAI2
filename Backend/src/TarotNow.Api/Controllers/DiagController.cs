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
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Controller)]
[Authorize(Roles = "admin")]
[ApiExplorerSettings(IgnoreApi = true)] // Ẩn khỏi Swagger - endpoint bí mật
public partial class DiagController : ControllerBase
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

}
