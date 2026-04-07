using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

public partial class DiagController
{
    /// <summary>
    /// ENDPOINT: POST /api/v1/Diag/seed-admin
    /// MỤC ĐÍCH: Tạo hoặc cập nhật tài khoản Super Admin.
    /// </summary>
    [HttpPost("seed-admin")]
    public async Task<IActionResult> SeedAdmin()
    {
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try
        {
            var result = await _diagnosticsService.SeedAdminAsync(HttpContext.RequestAborted);
            if (result.Status == SeedAdminStatus.InvalidConfiguration)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid seed-admin configuration",
                    detail: result.Message);
            }

            return Ok(new
            {
                Message = result.Message,
                Email = result.Email,
                Username = result.Username
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to seed admin account");
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Seed admin failed",
                detail: "Failed to seed admin account.");
        }
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Diag/seed-gamification
    /// MỤC ĐÍCH: Nạp dữ liệu init (quests, achievements, titles) cho Gamification.
    /// </summary>
    [HttpPost("seed-gamification")]
    public async Task<IActionResult> SeedGamification()
    {
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try
        {
            await _diagnosticsService.SeedGamificationDataAsync(HttpContext.RequestAborted);
            return Ok(new { message = "Gamification data seeded successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to seed gamification data");
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Seed gamification failed",
                detail: "Failed to seed gamification data.");
        }
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Diag/stats
    /// MỤC ĐÍCH: Xem thống kê dữ liệu MongoDB để kiểm tra hệ thống.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try
        {
            var stats = await _diagnosticsService.GetStatsAsync(HttpContext.RequestAborted);
            return Ok(new
            {
                stats.TotalSessionsInMongo,
                stats.TestUserSessions,
                stats.SampleDataRaw
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch diagnostics stats");
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Diagnostics query failed",
                detail: "Failed to fetch diagnostics stats.");
        }
    }
}
