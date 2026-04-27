using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Gamification.Commands;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Features.Gamification.Queries;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.AdminGamification)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị dữ liệu gamification.
// Luồng chính: quản lý quest/achievement/title và thao tác cấp danh hiệu hàng loạt.
public class AdminGamificationController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị gamification.
    /// </summary>
    /// <param name="mediator">MediatR dùng để dispatch nghiệp vụ.</param>
    public AdminGamificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy toàn bộ quest dành cho màn hình quản trị.
    /// </summary>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Danh sách quest hiện hành.</returns>
    [HttpGet("quests")]
    public async Task<IActionResult> GetAllQuests(CancellationToken ct)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetAllQuestsAdminQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới hoặc cập nhật định nghĩa quest.
    /// </summary>
    /// <param name="quest">Payload định nghĩa quest.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo lưu quest thành công.</returns>
    [HttpPost("quests")]
    public async Task<IActionResult> UpsertQuest([FromBody] QuestDefinitionDto quest, CancellationToken ct)
    {
        // Upsert giúp dashboard dùng cùng một endpoint cho create/update, giảm nhánh xử lý phía client.
        await _mediator.SendWithRequestCancellation(HttpContext, new UpsertQuestDefinitionCommand(quest), ct);
        return Ok(new { message = "Lưu Quest thành công." });
    }

    /// <summary>
    /// Xóa một quest theo mã định danh.
    /// </summary>
    /// <param name="code">Mã quest cần xóa.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo xóa quest thành công.</returns>
    [HttpDelete("quests/{code}")]
    public async Task<IActionResult> DeleteQuest(string code, CancellationToken ct)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, new DeleteQuestDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa Quest thành công." });
    }

    /// <summary>
    /// Lấy toàn bộ achievement cho trang quản trị.
    /// </summary>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Danh sách achievement hiện hành.</returns>
    [HttpGet("achievements")]
    public async Task<IActionResult> GetAllAchievements(CancellationToken ct)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetAllAchievementsAdminQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới hoặc cập nhật định nghĩa achievement.
    /// </summary>
    /// <param name="achievement">Payload định nghĩa achievement.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo lưu achievement thành công.</returns>
    [HttpPost("achievements")]
    public async Task<IActionResult> UpsertAchievement([FromBody] AchievementDefinitionDto achievement, CancellationToken ct)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, new UpsertAchievementDefinitionCommand(achievement), ct);
        return Ok(new { message = "Lưu Achievement thành công." });
    }

    /// <summary>
    /// Xóa một achievement theo mã định danh.
    /// </summary>
    /// <param name="code">Mã achievement cần xóa.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo xóa achievement thành công.</returns>
    [HttpDelete("achievements/{code}")]
    public async Task<IActionResult> DeleteAchievement(string code, CancellationToken ct)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, new DeleteAchievementDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa Achievement thành công." });
    }

    /// <summary>
    /// Lấy toàn bộ danh hiệu cho màn hình quản trị.
    /// </summary>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Danh sách danh hiệu hiện hành.</returns>
    [HttpGet("titles")]
    public async Task<IActionResult> GetAllTitles(CancellationToken ct)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetAllTitlesAdminQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới hoặc cập nhật định nghĩa danh hiệu.
    /// </summary>
    /// <param name="title">Payload định nghĩa danh hiệu.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo lưu danh hiệu thành công.</returns>
    [HttpPost("titles")]
    public async Task<IActionResult> UpsertTitle([FromBody] TitleDefinitionDto title, CancellationToken ct)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, new UpsertTitleDefinitionCommand(title), ct);
        return Ok(new { message = "Lưu Title thành công." });
    }

    /// <summary>
    /// Xóa một danh hiệu theo mã.
    /// </summary>
    /// <param name="code">Mã danh hiệu cần xóa.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo xóa danh hiệu thành công.</returns>
    [HttpDelete("titles/{code}")]
    public async Task<IActionResult> DeleteTitle(string code, CancellationToken ct)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, new DeleteTitleDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa danh hiệu thành công." });
    }

    /// <summary>
    /// Cấp toàn bộ danh hiệu hiện có cho một người dùng.
    /// </summary>
    /// <param name="userId">Id người dùng cần cấp danh hiệu.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông báo cấp danh hiệu thành công.</returns>
    [HttpPost("titles/grant-all/{userId}")]
    public async Task<IActionResult> GrantAllTitles(Guid userId, CancellationToken ct)
    {
        // Đây là thao tác có tác động rộng nên được tách command riêng để kiểm soát side effect.
        await _mediator.SendWithRequestCancellation(HttpContext, new GrantAllTitlesCommand(userId), ct);
        return Ok(new { message = "Đã cấp toàn bộ danh hiệu cho user." });
    }
}
