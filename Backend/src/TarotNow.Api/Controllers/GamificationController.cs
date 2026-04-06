using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Gamification.Commands;
using TarotNow.Application.Features.Gamification.Queries;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Helpers;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Gamification)]
[Authorize]
public class GamificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITitleRepository _titleRepository;
    private readonly ILeaderboardRepository _leaderboardRepository;
    private readonly MongoDbContext _mongoContext;

    public GamificationController(
        IMediator mediator, 
        ITitleRepository titleRepository,
        ILeaderboardRepository leaderboardRepository,
        MongoDbContext mongoContext)
    {
        _mediator = mediator;
        _titleRepository = titleRepository;
        _leaderboardRepository = leaderboardRepository;
        _mongoContext = mongoContext;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // === QUESTS ===
    [HttpGet("quests")]
    public async Task<IActionResult> GetActiveQuests([FromQuery] string type = "daily")
    {
        var query = new GetActiveQuestsQuery(GetUserId(), type);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("quests/{questCode}/claim")]
    public async Task<IActionResult> ClaimQuestReward(string questCode, [FromBody] ClaimQuestRewardRequest req)
    {
        var command = new ClaimQuestRewardCommand(GetUserId(), questCode, req.PeriodKey);
        var result = await _mediator.Send(command);
        
        if (!result.Success && result.AlreadyClaimed) 
            return Problem(statusCode: 400, detail: "QUEST_ALREADY_CLAIMED");
            
        if (!result.Success) 
            return Problem(statusCode: 400, detail: "QUEST_NOT_COMPLETED_OR_FAILED");
        
        return Ok(result);
    }

    // === ACHIEVEMENTS ===
    [HttpGet("achievements")]
    public async Task<IActionResult> GetAchievements()
    {
        var query = new GetUserAchievementsQuery(GetUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // === TITLES ===
    [HttpGet("titles")]
    public async Task<IActionResult> GetTitles()
    {
        var query = new GetUserTitlesQuery(GetUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("titles/active")]
    public async Task<IActionResult> SetActiveTitle([FromBody] SetActiveTitleRequest req)
    {
        var command = new SetActiveTitleCommand(GetUserId(), req.TitleCode);
        var success = await _mediator.Send(command);
        if (!success) return BadRequest(new { message = "Bạn không sở hữu danh hiệu này." });
        return Ok(new { message = "Cập nhật danh hiệu thành công." });
    }

    [HttpPost("sandbox-grant-me")]
    public async Task<IActionResult> SandboxGrantMeAllTitles(CancellationToken ct)
    {
        var userId = GetUserId();
        var titles = await _titleRepository.GetAllTitlesAsync(ct);
        foreach (var title in titles)
        {
            if (!await _titleRepository.OwnsTitleAsync(userId, title.Code, ct))
            {
                await _titleRepository.GrantTitleAsync(userId, title.Code, ct);
            }
        }
        return Ok(new { message = "Dev Sandbox: Chúc mừng bạn đã được ban tặng toàn quyền sở hữu toàn bộ danh hiệu trong game!" });
    }

    [HttpDelete("sandbox-purge-leaderboard")]
    public async Task<IActionResult> SandboxPurgeLeaderboard(CancellationToken ct)
    {
        // Xóa trắng dữ liệu bảng xếp hạng để reset môi trường test
        await _mongoContext.LeaderboardEntries.DeleteManyAsync(FilterDefinition<LeaderboardEntryDocument>.Empty, ct);
        await _mongoContext.LeaderboardSnapshots.DeleteManyAsync(FilterDefinition<LeaderboardSnapshotDocument>.Empty, ct);
        return Ok(new { message = "Đã xóa toàn bộ dữ liệu bảng xếp hạng." });
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] string track = "daily_rank_score", [FromQuery] string? periodKey = null)
    {
        var query = new GetLeaderboardQuery(GetUserId(), track, periodKey);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // === DEBUG & SANDBOX ===
    [HttpGet("debug-stats")]
    [AllowAnonymous] // Tạm thời cho phép anonymous để test nhanh hoặc có thể giữ Authorize tùy ý
    public async Task<IActionResult> GetDebugStats()
    {
        var totalEntries = await _mongoContext.LeaderboardEntries.CountDocumentsAsync(FilterDefinition<LeaderboardEntryDocument>.Empty);
        var totalSnapshots = await _mongoContext.LeaderboardSnapshots.CountDocumentsAsync(FilterDefinition<LeaderboardSnapshotDocument>.Empty);
        
        // Lấy 5 bản ghi mới nhất để xem track name thực tế
        var latestEntries = await _mongoContext.LeaderboardEntries.Find(FilterDefinition<LeaderboardEntryDocument>.Empty)
            .SortByDescending(x => x.UpdatedAt)
            .Limit(5)
            .ToListAsync();

        return Ok(new 
        { 
            TotalEntries = totalEntries, 
            TotalSnapshots = totalSnapshots,
            LatestEntries = latestEntries.Select(e => new { e.ScoreTrack, e.PeriodKey, e.Score, e.UserId })
        });
    }

    // Endpoint phục vụ việc kiểm tra dữ liệu mẫu (Sẽ được bảo mật lại sau khi test thành công)
    [AllowAnonymous]
    [HttpPost("sandbox-grant-score")]
    public async Task<IActionResult> SandboxGrantScore([FromQuery] string track = "spent_gold", [FromQuery] long points = 1000)
    {
        // Phục vụ việc kiểm tra tự động khi chưa đăng nhập. 
        // Fallback về ID của SuperAdmin nếu không tìm thấy UserID trong Token.
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = string.IsNullOrEmpty(userIdStr) 
            ? Guid.Parse("86e87b7b-bf14-46a1-927a-a41899f3057b") 
            : Guid.Parse(userIdStr);
            
        var dailyKey = PeriodKeyHelper.GetPeriodKey("daily");
        var monthlyKey = PeriodKeyHelper.GetPeriodKey("monthly");

        await _leaderboardRepository.IncrementScoreAsync(userId, track, dailyKey, points, default);
        await _leaderboardRepository.IncrementScoreAsync(userId, track, monthlyKey, points, default);
        await _leaderboardRepository.IncrementScoreAsync(userId, track, "all", points, default);

        return Ok(new { message = $"Đã cộng {points} điểm vào track '{track}' cho user {userId}" });
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
