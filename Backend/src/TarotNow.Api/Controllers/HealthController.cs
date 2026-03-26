/*
 * ===================================================================
 * FILE: HealthController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller kiểm tra TRẠNG THÁI SỐNG (health check) của API server.
 *
 * HEALTH CHECK LÀ GÌ?
 *   Giống như bác sĩ khám sức khỏe định kỳ cho server:
 *   - "Server còn sống không?" → gọi GET /api/v1/Health
 *   - Trả về "Healthy" = server đang hoạt động bình thường
 *   - Không trả về / trả lỗi = server có vấn đề
 *
 * AI DÙNG?
 *   1. Load Balancer (Nginx, AWS ALB): gọi health check mỗi 10-30 giây.
 *      Nếu server không phản hồi → chuyển traffic sang server khác.
 *   2. Monitoring tools (Grafana, Datadog): theo dõi uptime.
 *   3. Orchestrator (Kubernetes): tự động restart container nếu unhealthy.
 *   4. DevOps team: kiểm tra nhanh sau deploy.
 *
 * KHÔNG CẦN AUTHORIZE:
 *   Health check endpoint thường public (không cần đăng nhập)
 *   vì load balancer/monitoring tools không có JWT token.
 * ===================================================================
 */

using Microsoft.AspNetCore.Authorization; // [Authorize] cho endpoint test
using Microsoft.AspNetCore.Mvc;           // API controller base

namespace TarotNow.Api.Controllers;

/*
 * URL gốc: /api/v1/Health
 * KHÔNG có [Authorize] ở cấp class → endpoint health check ai cũng gọi được.
 */
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Controller)]
public class HealthController : ControllerBase
{
    /*
     * _logger: ghi log khi health check được gọi.
     *   Hữu ích để biết tần suất load balancer kiểm tra.
     * _environment: biết môi trường hiện tại (Dev/Prod).
     */
    private readonly ILogger<HealthController> _logger;
    private readonly IWebHostEnvironment _environment;

    public HealthController(ILogger<HealthController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Health
    /// MỤC ĐÍCH: Kiểm tra API server có đang hoạt động không.
    ///
    /// RESPONSE:
    ///   {
    ///     "Status": "Healthy",       → Server OK
    ///     "Timestamp": "2026-03-19T08:00:00Z", → Thời điểm kiểm tra
    ///     "Version": "1.0"           → Phiên bản API
    ///   }
    ///
    /// HTTP 200 OK = server sống, HTTP 5xx = server có vấn đề.
    ///
    /// Lưu ý: Hàm này KHÔNG phải async (không có từ khóa async)
    /// vì không cần truy vấn database hay gọi service ngoài.
    /// Chỉ trả về thông tin tĩnh → nhanh nhất có thể.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        // Ghi log mỗi khi health check được gọi
        _logger.LogInformation("Health check endpoint was called.");

        return Ok(new
        {
            Status = "Healthy",              // Trạng thái server
            Timestamp = DateTime.UtcNow,     // Thời điểm hiện tại (UTC - giờ quốc tế)
            Version = "1.0"                  // Phiên bản API đang chạy
        });
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Health/error-test
    /// MỤC ĐÍCH: Cố tình gây ra lỗi để kiểm tra hệ thống xử lý lỗi (error handling).
    ///
    /// TẠI SAO CẦN?
    ///   Khi phát triển, cần kiểm tra:
    ///   - GlobalExceptionHandler có bắt được exception không?
    ///   - Response lỗi có đúng format ProblemDetails không?
    ///   - Log có ghi lại thông tin lỗi đầy đủ không?
    ///
    /// BẢO MẬT:
    ///   - [Authorize(Roles = "admin")]: chỉ admin mới gọi được
    ///   - Kiểm tra IsDevelopment(): chỉ chạy trong môi trường Development
    ///   - Trong Production: trả 404 Not Found (giả vờ không tồn tại)
    /// </summary>
    [HttpGet("error-test")]
    [Authorize(Roles = "admin")]
    public IActionResult TriggerError()
    {
        // Chỉ cho phép trong Development
        if (!_environment.IsDevelopment())
            return NotFound();

        // Cố tình throw exception → GlobalExceptionHandler sẽ bắt và trả ProblemDetails
        throw new Exception("This is a test exception to verify ProblemDetails integration.");
    }
}
