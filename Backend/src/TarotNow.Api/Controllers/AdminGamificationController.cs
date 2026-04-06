using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Features.Gamification.Queries;
using TarotNow.Application.Features.Gamification.Commands;
using TarotNow.Application.Interfaces;
using System.Linq;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.AdminGamification)]
[Authorize(Roles = "admin")]
public class AdminGamificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITitleRepository _titleRepository;

    public AdminGamificationController(IMediator mediator, ITitleRepository titleRepository)
    {
        _mediator = mediator;
        _titleRepository = titleRepository;
    }

    // === QUESTS ===
    [HttpGet("quests")]
    public async Task<IActionResult> GetAllQuests(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllQuestsAdminQuery(), ct);
        return Ok(result);
    }

    [HttpPost("quests")]
    public async Task<IActionResult> UpsertQuest([FromBody] QuestDefinitionDto quest, CancellationToken ct)
    {
        await _mediator.Send(new UpsertQuestDefinitionCommand(quest), ct);
        return Ok(new { message = "Lưu Quest thành công." });
    }

    [HttpDelete("quests/{code}")]
    public async Task<IActionResult> DeleteQuest(string code, CancellationToken ct)
    {
        await _mediator.Send(new DeleteQuestDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa Quest thành công." });
    }

    // === ACHIEVEMENTS ===
    [HttpGet("achievements")]
    public async Task<IActionResult> GetAllAchievements(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAchievementsAdminQuery(), ct);
        return Ok(result);
    }

    [HttpPost("achievements")]
    public async Task<IActionResult> UpsertAchievement([FromBody] AchievementDefinitionDto achievement, CancellationToken ct)
    {
        await _mediator.Send(new UpsertAchievementDefinitionCommand(achievement), ct);
        return Ok(new { message = "Lưu Achievement thành công." });
    }

    [HttpDelete("achievements/{code}")]
    public async Task<IActionResult> DeleteAchievement(string code, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAchievementDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa Achievement thành công." });
    }

    // === TITLES ===
    [HttpGet("titles")]
    public async Task<IActionResult> GetAllTitles(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTitlesAdminQuery(), ct);
        return Ok(result);
    }

    [HttpPost("titles")]
    public async Task<IActionResult> UpsertTitle([FromBody] TitleDefinitionDto title, CancellationToken ct)
    {
        await _mediator.Send(new UpsertTitleDefinitionCommand(title), ct);
        return Ok(new { message = "Lưu Title thành công." });
    }

    [HttpDelete("titles/{code}")]
    public async Task<IActionResult> DeleteTitle(string code, CancellationToken ct)
    {
        await _mediator.Send(new DeleteTitleDefinitionCommand(code), ct);
        return Ok(new { message = "Xóa danh hiệu thành công." });
    }

    [HttpPost("titles/grant-all/{userId}")]
    public async Task<IActionResult> GrantAllTitles(Guid userId, CancellationToken ct)
    {
        var titles = await _titleRepository.GetAllTitlesAsync(ct);
        foreach (var title in titles)
        {
            if (!await _titleRepository.OwnsTitleAsync(userId, title.Code, ct))
            {
                await _titleRepository.GrantTitleAsync(userId, title.Code, ct);
            }
        }
        return Ok(new { message = "Đã cấp toàn bộ danh hiệu cho user." });
    }
}
