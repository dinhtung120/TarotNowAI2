

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

    private IActionResult? RejectIfNotDevelopment()
    {
        
        if (_environment.IsDevelopment()) return null; 
        return this.NotFoundProblem("Endpoint chỉ khả dụng trong môi trường development.");
    }

        [HttpPost("wipe")]
    public IActionResult Wipe()
    {
        
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard; 

        
        return Ok(new { message = "Wipe endpoint is disabled by default." });
    }

}
