
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;           
using Microsoft.AspNetCore.RateLimiting;
using System.Reflection;
using TarotNow.Api.Extensions;
using TarotNow.Application.Interfaces;

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
    private readonly IReadinessService _readinessService;

    /// <summary>
    /// Khởi tạo controller health check.
    /// </summary>
    /// <param name="logger">Logger phục vụ quan sát truy cập health endpoint.</param>
    /// <param name="environment">Thông tin môi trường chạy hiện tại.</param>
    /// <param name="readinessService">Service kiểm tra readiness dependencies.</param>
    public HealthController(
        ILogger<HealthController> logger,
        IWebHostEnvironment environment,
        IReadinessService readinessService)
    {
        _logger = logger;
        _environment = environment;
        _readinessService = readinessService;
    }

    /// <summary>
    /// Trả trạng thái sống của API.
    /// </summary>
    /// <returns>Thông tin health cơ bản gồm trạng thái, thời gian và version.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Live();
    }

    /// <summary>
    /// Trả trạng thái sống (liveness) của API process.
    /// </summary>
    /// <returns>Payload liveness ổn định cho load balancer.</returns>
    [HttpGet("live")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Live()
    {
        _logger.LogDebug("Health liveness endpoint was called.");

        var assembly = typeof(HealthController).Assembly;
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                      ?? assembly.GetName().Version?.ToString()
                      ?? "unknown";

        return Ok(new
        {
            Status = "Healthy",              
            Timestamp = DateTime.UtcNow,     
            Version = version
        });
    }

    /// <summary>
    /// Trả trạng thái sẵn sàng (readiness) dựa trên kết nối dependency chính.
    /// Luồng xử lý: kiểm tra PostgreSQL, MongoDB và Redis rồi trả 200/503 tương ứng.
    /// </summary>
    [HttpGet("ready")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Ready(CancellationToken cancellationToken)
    {
        var checks = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        var readiness = await _readinessService.CheckAsync(cancellationToken);
        checks["postgresql"] = new { status = readiness.PostgreSqlReady ? "Healthy" : "Unhealthy" };
        checks["mongodb"] = new { status = readiness.MongoDbReady ? "Healthy" : "Unhealthy" };
        checks["redis"] = new
        {
            status = readiness.RedisRequired
                ? (readiness.RedisReady ? "Healthy" : "Unhealthy")
                : "Skipped",
            required = readiness.RedisRequired
        };
        checks["aiProvider"] = new
        {
            status = readiness.AiProviderReady ? "Healthy" : "Unhealthy",
            message = readiness.AiProviderMessage
        };

        var allReady = readiness.PostgreSqlReady
                       && readiness.MongoDbReady
                       && readiness.AiProviderReady
                       && (!readiness.RedisRequired || readiness.RedisReady);
        var payload = new
        {
            Status = allReady ? "Ready" : "NotReady",
            Timestamp = DateTime.UtcNow,
            RedisRequired = readiness.RedisRequired,
            Checks = checks
        };

        return allReady ? Ok(payload) : StatusCode(StatusCodes.Status503ServiceUnavailable, payload);
    }

    /// <summary>
    /// Endpoint test luồng xử lý exception toàn cục.
    /// Luồng xử lý: chỉ cho phép trong development, sau đó chủ động ném exception kiểm thử.
    /// </summary>
    /// <returns>Không có giá trị trả về thành công; endpoint dùng để kiểm tra xử lý lỗi.</returns>
    [HttpGet("error-test")]
    [Authorize(Roles = "admin")]
    [EnableRateLimiting("auth-session")]
    public IActionResult TriggerError()
    {
        // Chặn endpoint test lỗi trên môi trường non-development để tránh hành vi nguy hiểm.
        if (!_environment.IsDevelopment())
            return this.NotFoundProblem("Endpoint chỉ khả dụng trong môi trường development.");

        // Chủ động ném lỗi để kiểm thử pipeline ProblemDetails và logging.
        throw new Exception("This is a test exception to verify ProblemDetails integration.");
    }
}
