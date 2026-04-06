using MediatR;
using Microsoft.AspNetCore.Authorization;
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
public class AdminGamificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminGamificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Lấy toàn bộ quest definitions cho màn hình quản trị.</summary>
    [HttpGet("quests")]
    public async Task<IActionResult> GetAllQuests(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllQuestsAdminQuery(), ct);
        return Ok(result);
    }

    /// <summary>Tạo mới hoặc cập nhật quest definition.</summary>
    [HttpPost("quests")]
    public async Task<IActionResult> UpsertQuest([FromBody] QuestDefinitionDto quest, CancellationToken ct)
    {
        await _mediator.Send(new UpsertQuestDefinitionCommand(quest), ct);
        return Ok(new { message = "Lưu Quest thành công." });
    }

    /// <summary>Xóa quest definition theo mã.</summary>
    [HttpDelete("quests/{code}")]
    public async Task<IActionResult> DeleteQuest(string code, CancellationToken ct)
    {
        await _mediator.Send(new DeleteQuestDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa Quest thành công." });
    }

    /// <summary>Lấy toàn bộ achievement definitions cho quản trị.</summary>
    [HttpGet("achievements")]
    public async Task<IActionResult> GetAllAchievements(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAchievementsAdminQuery(), ct);
        return Ok(result);
    }

    /// <summary>Tạo mới hoặc cập nhật achievement definition.</summary>
    [HttpPost("achievements")]
    public async Task<IActionResult> UpsertAchievement([FromBody] AchievementDefinitionDto achievement, CancellationToken ct)
    {
        await _mediator.Send(new UpsertAchievementDefinitionCommand(achievement), ct);
        return Ok(new { message = "Lưu Achievement thành công." });
    }

    /// <summary>Xóa achievement definition theo mã.</summary>
    [HttpDelete("achievements/{code}")]
    public async Task<IActionResult> DeleteAchievement(string code, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAchievementDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa Achievement thành công." });
    }

    /// <summary>Lấy toàn bộ title definitions cho quản trị.</summary>
    [HttpGet("titles")]
    public async Task<IActionResult> GetAllTitles(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTitlesAdminQuery(), ct);
        return Ok(result);
    }

    /// <summary>Tạo mới hoặc cập nhật title definition.</summary>
    [HttpPost("titles")]
    public async Task<IActionResult> UpsertTitle([FromBody] TitleDefinitionDto title, CancellationToken ct)
    {
        await _mediator.Send(new UpsertTitleDefinitionCommand(title), ct);
        return Ok(new { message = "Lưu Title thành công." });
    }

    /// <summary>Xóa title definition theo mã.</summary>
    [HttpDelete("titles/{code}")]
    public async Task<IActionResult> DeleteTitle(string code, CancellationToken ct)
    {
        await _mediator.Send(new DeleteTitleDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa danh hiệu thành công." });
    }

    /// <summary>Cấp toàn bộ danh hiệu hiện có cho một người dùng.</summary>
    [HttpPost("titles/grant-all/{userId}")]
    public async Task<IActionResult> GrantAllTitles(Guid userId, CancellationToken ct)
    {
        await _mediator.Send(new GrantAllTitlesCommand(userId), ct);
        return Ok(new { message = "Đã cấp toàn bộ danh hiệu cho user." });
    }
}
