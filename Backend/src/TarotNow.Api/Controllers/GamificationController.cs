using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Gamification.Commands;
using TarotNow.Application.Features.Gamification.Queries;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Gamification)]
[Authorize]
[EnableRateLimiting("auth-session")]
// API gamification phía người dùng.
// Luồng chính: truy vấn quest/achievement/title, nhận thưởng quest và đặt danh hiệu đang dùng.
public class GamificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo controller gamification.
    /// </summary>
    /// <param name="mediator">MediatR điều phối nghiệp vụ gamification.</param>
    /// <param name="systemConfigSettings">Nguồn policy runtime cho quest type và leaderboard track mặc định.</param>
    public GamificationController(
        IMediator mediator,
        ISystemConfigSettings systemConfigSettings)
    {
        _mediator = mediator;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Lấy user id hiện tại từ claim định danh.
    /// </summary>
    /// <returns>User id của request hiện tại.</returns>
    private Guid GetUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(rawUserId, out var userId) || userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("Invalid authenticated user context.");
        }

        return userId;
    }

    /// <summary>
    /// Lấy danh sách quest đang hoạt động theo loại.
    /// </summary>
    /// <param name="type">Loại quest cần lấy (daily, weekly...).</param>
    /// <returns>Danh sách quest active của người dùng.</returns>
    [HttpGet("quests")]
    public async Task<IActionResult> GetActiveQuests([FromQuery] string? type = null)
    {
        var resolvedType = string.IsNullOrWhiteSpace(type)
            ? _systemConfigSettings.GamificationDefaultQuestType
            : type.Trim().ToLowerInvariant();
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetActiveQuestsQuery(GetUserId(), resolvedType));
        return Ok(result);
    }

    /// <summary>
    /// Nhận thưởng quest.
    /// Luồng xử lý: gọi command claim và rẽ nhánh lỗi theo từng trạng thái nghiệp vụ.
    /// </summary>
    /// <param name="questCode">Mã quest cần claim.</param>
    /// <param name="request">Payload period key của quest.</param>
    /// <returns>Kết quả claim hoặc lỗi business tương ứng.</returns>
    [HttpPost("quests/{questCode}/claim")]
    public async Task<IActionResult> ClaimQuestReward(string questCode, [FromBody] ClaimQuestRewardRequest request)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new ClaimQuestRewardCommand(GetUserId(), questCode, request.PeriodKey));
        // Tách mã lỗi chi tiết để client phân biệt quest đã claim và quest chưa đủ điều kiện.
        if (!result.Success && result.AlreadyClaimed) return Problem(statusCode: 400, detail: "QUEST_ALREADY_CLAIMED");
        if (!result.Success) return Problem(statusCode: 400, detail: "QUEST_NOT_COMPLETED_OR_FAILED");
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách achievement của người dùng.
    /// </summary>
    /// <returns>Danh sách achievement đã mở khóa/chưa mở khóa.</returns>
    [HttpGet("achievements")]
    public async Task<IActionResult> GetAchievements()
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetUserAchievementsQuery(GetUserId()));
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách danh hiệu của người dùng.
    /// </summary>
    /// <returns>Danh sách title người dùng sở hữu.</returns>
    [HttpGet("titles")]
    public async Task<IActionResult> GetTitles()
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetUserTitlesQuery(GetUserId()));
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật danh hiệu đang hiển thị của người dùng.
    /// </summary>
    /// <param name="request">Payload title code cần kích hoạt.</param>
    /// <returns>Thông báo thành công hoặc lỗi khi title không thuộc sở hữu user.</returns>
    [HttpPost("titles/active")]
    public async Task<IActionResult> SetActiveTitle([FromBody] SetActiveTitleRequest request)
    {
        var success = await _mediator.SendWithRequestCancellation(HttpContext, new SetActiveTitleCommand(GetUserId(), request.TitleCode));
        // Rule nghiệp vụ: chỉ cho phép set active title khi user đã sở hữu title đó.
        if (!success) return Problem(statusCode: 400, detail: "TITLE_NOT_OWNED");
        return Ok(new { message = "Cập nhật danh hiệu thành công." });
    }

    /// <summary>
    /// Lấy bảng xếp hạng theo track và period.
    /// </summary>
    /// <param name="track">Track leaderboard cần truy vấn.</param>
    /// <param name="periodKey">Mốc thời gian leaderboard tùy chọn.</param>
    /// <returns>Dữ liệu bảng xếp hạng cho người dùng hiện tại.</returns>
    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] string? track = null, [FromQuery] string? periodKey = null)
    {
        var resolvedTrack = string.IsNullOrWhiteSpace(track)
            ? _systemConfigSettings.GamificationDefaultLeaderboardTrack
            : track.Trim().ToLowerInvariant();
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetLeaderboardQuery(GetUserId(), resolvedTrack, periodKey));
        return Ok(result);
    }
}

// Payload nhận thưởng quest theo period cụ thể.
public class ClaimQuestRewardRequest
{
    // Khóa chu kỳ quest cần claim (ví dụ ngày/tuần cụ thể).
    public string PeriodKey { get; set; } = string.Empty;
}

// Payload cập nhật danh hiệu đang hiển thị.
public class SetActiveTitleRequest
{
    // Mã danh hiệu cần đặt làm active title.
    public string TitleCode { get; set; } = string.Empty;
}
