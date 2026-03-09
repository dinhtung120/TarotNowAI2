using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Kiểm tra trạng thái hoạt động của API.
    /// Trả về 200 OK cùng timestamp nếu service đang chạy.
    /// </summary>
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
    /// Test endpoint để kiểm tra global error handling (ProblemDetails).
    /// </summary>
    [HttpGet("error-test")]
    public IActionResult TriggerError()
    {
        throw new Exception("This is a test exception to verify ProblemDetails integration.");
    }
}
