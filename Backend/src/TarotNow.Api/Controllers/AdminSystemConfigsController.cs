using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị system configs.
// Luồng chính: liệt kê key cấu hình chuẩn hóa và cập nhật từng key với validate theo registry.
public sealed class AdminSystemConfigsController : ControllerBase
{
    private readonly ISystemConfigAdminService _systemConfigAdminService;

    /// <summary>
    /// Khởi tạo controller quản trị system config.
    /// </summary>
    public AdminSystemConfigsController(
        ISystemConfigAdminService systemConfigAdminService)
    {
        _systemConfigAdminService = systemConfigAdminService;
    }

    /// <summary>
    /// Lấy danh sách key cấu hình để hiển thị trên admin dashboard.
    /// Luồng xử lý: merge registry key chuẩn với dữ liệu đang có trong DB.
    /// </summary>
    [HttpGet("system-configs")]
    public async Task<IActionResult> ListSystemConfigs(CancellationToken cancellationToken)
    {
        var items = await _systemConfigAdminService.ListAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Cập nhật một key cấu hình và refresh snapshot runtime.
    /// Luồng xử lý: authorize admin, validate payload theo registry, lưu DB rồi đồng bộ projection.
    /// </summary>
    [HttpPut("system-configs/{key}")]
    public async Task<IActionResult> UpsertSystemConfig(
        [FromRoute] string key,
        [FromBody] UpsertSystemConfigRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var adminId))
        {
            return this.UnauthorizedProblem();
        }

        if (request is null || string.IsNullOrWhiteSpace(request.Value))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid request",
                detail: "Value is required.");
        }

        if (!SystemConfigRegistry.TryGetDefinition(key, out var definition))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Unknown system config key",
                detail: $"Key '{SystemConfigRegistry.NormalizeKey(key)}' is not supported.");
        }

        var normalizedKind = SystemConfigRegistry.NormalizeValueKind(
            string.IsNullOrWhiteSpace(request.ValueKind)
                ? definition.ValueKind.ToString()
                : request.ValueKind);

        var validation = SystemConfigRegistry.Validate(definition.Key, request.Value, normalizedKind);
        if (!validation.IsValid)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid system config value",
                detail: validation.Error);
        }

        var updated = await _systemConfigAdminService.UpsertAsync(
            definition.Key,
            request.Value,
            normalizedKind,
            request.Description,
            adminId,
            cancellationToken);
        return Ok(updated);
    }

    public sealed class UpsertSystemConfigRequest
    {
        public string Value { get; set; } = string.Empty;
        public string? ValueKind { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// Khởi động lại ứng dụng (restart process).
    /// Sử dụng IHostApplicationLifetime.StopApplication() để đóng tiến trình hiện tại.
    /// Thông thường Docker hoặc hệ thống quản lý (Systemd, IIS) sẽ tự động bật lại ứng dụng.
    /// Điều này hữu ích khi cần áp dụng ngay lập tức các giá trị cấu hình chưa được live-reload.
    /// </summary>
    [HttpPost("system-configs/restart")]
    public IActionResult RestartServer([FromServices] IHostApplicationLifetime lifetime)
    {
        // Gửi tín hiệu dừng ứng dụng
        lifetime.StopApplication();
        return Ok(new { message = "Server is restarting..." });
    }
}
