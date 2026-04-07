using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Gamification.Commands;
using TarotNow.Application.Features.Gamification.Queries;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Gamification)]
[Authorize]
public class GamificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public GamificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("quests")]
    public async Task<IActionResult> GetActiveQuests([FromQuery] string type = "daily")
    {
        var result = await _mediator.Send(new GetActiveQuestsQuery(GetUserId(), type));
        return Ok(result);
    }

        [HttpPost("quests/{questCode}/claim")]
    public async Task<IActionResult> ClaimQuestReward(string questCode, [FromBody] ClaimQuestRewardRequest request)
    {
        var result = await _mediator.Send(new ClaimQuestRewardCommand(GetUserId(), questCode, request.PeriodKey));
        if (!result.Success && result.AlreadyClaimed) return Problem(statusCode: 400, detail: "QUEST_ALREADY_CLAIMED");
        if (!result.Success) return Problem(statusCode: 400, detail: "QUEST_NOT_COMPLETED_OR_FAILED");
        return Ok(result);
    }

        [HttpGet("achievements")]
    public async Task<IActionResult> GetAchievements()
    {
        var result = await _mediator.Send(new GetUserAchievementsQuery(GetUserId()));
        return Ok(result);
    }

        [HttpGet("titles")]
    public async Task<IActionResult> GetTitles()
    {
        var result = await _mediator.Send(new GetUserTitlesQuery(GetUserId()));
        return Ok(result);
    }

        [HttpPost("titles/active")]
    public async Task<IActionResult> SetActiveTitle([FromBody] SetActiveTitleRequest request)
    {
        var success = await _mediator.Send(new SetActiveTitleCommand(GetUserId(), request.TitleCode));
        if (!success) return Problem(statusCode: 400, detail: "TITLE_NOT_OWNED");
        return Ok(new { message = "Cập nhật danh hiệu thành công." });
    }

        [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] string track = "daily_rank_score", [FromQuery] string? periodKey = null)
    {
        var result = await _mediator.Send(new GetLeaderboardQuery(GetUserId(), track, periodKey));
        return Ok(result);
    }
}

public class ClaimQuestRewardRequest
{
    public string PeriodKey { get; set; } = string.Empty;
}

public class SetActiveTitleRequest
{
    public string TitleCode { get; set; } = string.Empty;
}
