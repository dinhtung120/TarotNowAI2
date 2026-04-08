

using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;           
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;


[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Controller)]
// API kiểm tra sức khỏe dịch vụ.
// Luồng chính: trả trạng thái health cơ bản và endpoint test lỗi cho admin trong môi trường dev.
public class HealthController : ControllerBase
{
    
    private readonly ILogger<HealthController> _logger;
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Khởi tạo controller health check.
    /// </summary>
    /// <param name="logger">Logger phục vụ quan sát truy cập health endpoint.</param>
    /// <param name="environment">Thông tin môi trường chạy hiện tại.</param>
    public HealthController(ILogger<HealthController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Trả trạng thái sống của API.
    /// </summary>
    /// <returns>Thông tin health cơ bản gồm trạng thái, thời gian và version.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        
        _logger.LogInformation("Health check endpoint was called.");

        return Ok(new
        {
            Status = "Healthy",              
            Timestamp = DateTime.UtcNow,     
            Version = "1.0"                  
        });
    }

    /// <summary>
    /// Endpoint test luồng xử lý exception toàn cục.
    /// Luồng xử lý: chỉ cho phép trong development, sau đó chủ động ném exception kiểm thử.
    /// </summary>
    /// <returns>Không có giá trị trả về thành công; endpoint dùng để kiểm tra xử lý lỗi.</returns>
    [HttpGet("error-test")]
    [Authorize(Roles = "admin")]
    public IActionResult TriggerError()
    {
        // Chặn endpoint test lỗi trên môi trường non-development để tránh hành vi nguy hiểm.
        if (!_environment.IsDevelopment())
            return this.NotFoundProblem("Endpoint chỉ khả dụng trong môi trường development.");

        // Chủ động ném lỗi để kiểm thử pipeline ProblemDetails và logging.
        throw new Exception("This is a test exception to verify ProblemDetails integration.");
    }
}
