using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

public partial class DiagController
{
    /// <summary>
    /// Seed tài khoản admin cho môi trường development.
    /// Luồng xử lý: kiểm tra guard môi trường, gọi service seed, rẽ nhánh theo kết quả cấu hình.
    /// </summary>
    /// <returns>Thông tin admin seed thành công hoặc lỗi cấu hình/hạ tầng.</returns>
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
                // Trả lỗi cấu hình rõ ràng để dev sửa env/secret trước khi thử seed lại.
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
            // Log lỗi seed để phục vụ chẩn đoán nguyên nhân thất bại trong môi trường dev.
            _logger.LogError(ex, "Failed to seed admin account");
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Seed admin failed",
                detail: "Failed to seed admin account.");
        }
    }

    /// <summary>
    /// Seed dữ liệu gamification mẫu cho môi trường development.
    /// </summary>
    /// <returns>Thông báo seed thành công hoặc lỗi nội bộ khi seed thất bại.</returns>
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
            // Log lỗi để trace nhanh khi seed dữ liệu mẫu thất bại.
            _logger.LogError(ex, "Failed to seed gamification data");
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Seed gamification failed",
                detail: "Failed to seed gamification data.");
        }
    }

    /// <summary>
    /// Lấy thống kê chẩn đoán phục vụ kiểm tra dữ liệu test.
    /// </summary>
    /// <returns>Bộ thống kê diagnostics hoặc lỗi nội bộ nếu truy vấn thất bại.</returns>
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
            // Log lỗi truy vấn stats để giảm thời gian điều tra khi môi trường dev gặp sự cố.
            _logger.LogError(ex, "Failed to fetch diagnostics stats");
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Diagnostics query failed",
                detail: "Failed to fetch diagnostics stats.");
        }
    }
}
