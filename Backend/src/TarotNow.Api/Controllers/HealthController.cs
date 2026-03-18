using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;
    private readonly IWebHostEnvironment _environment;

    public HealthController(ILogger<HealthController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
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
    [Authorize(Roles = "admin")]
    public IActionResult TriggerError()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        throw new Exception("This is a test exception to verify ProblemDetails integration.");
    }
}
