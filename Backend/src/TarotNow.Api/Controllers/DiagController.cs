

using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;           

using TarotNow.Application.Interfaces;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;


[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Controller)]
[Authorize(Roles = "admin")]
[ApiExplorerSettings(IgnoreApi = true)] 
// API chẩn đoán nội bộ dành cho môi trường development.
// Luồng chính: cung cấp các endpoint hỗ trợ seed/test nhưng chặn ở non-development.
public partial class DiagController : ControllerBase
{
    private readonly IDiagnosticsService _diagnosticsService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DiagController> _logger;

    /// <summary>
    /// Khởi tạo controller chẩn đoán nội bộ.
    /// </summary>
    /// <param name="diagnosticsService">Service thực thi nghiệp vụ seed/stats.</param>
    /// <param name="environment">Thông tin môi trường chạy hiện tại.</param>
    /// <param name="logger">Logger phục vụ quan sát lỗi diagnostic.</param>
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
    /// Chặn endpoint khi không chạy trong môi trường development.
    /// Luồng xử lý: cho phép tiếp tục ở dev, trả NotFound ở môi trường còn lại để tránh lộ endpoint nội bộ.
    /// </summary>
    /// <returns><c>null</c> nếu được phép chạy; ngược lại trả IActionResult lỗi.</returns>
    private IActionResult? RejectIfNotDevelopment()
    {
        // Guard môi trường để mọi endpoint chẩn đoán không vô tình mở trên production.
        if (_environment.IsDevelopment()) return null; 
        return this.NotFoundProblem("Endpoint chỉ khả dụng trong môi trường development.");
    }

    /// <summary>
    /// Endpoint mẫu cho thao tác wipe dữ liệu (đang tắt mặc định).
    /// </summary>
    /// <returns>Thông báo endpoint đã bị vô hiệu hóa mặc định.</returns>
    [HttpPost("wipe")]
    public IActionResult Wipe()
    {
        // Bảo vệ endpoint theo môi trường trước khi thực hiện bất kỳ thao tác phá hủy nào.
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard; 

        // Chủ động trả thông báo disabled để tránh xóa dữ liệu ngoài ý muốn.
        return Ok(new { message = "Wipe endpoint is disabled by default." });
    }

}
