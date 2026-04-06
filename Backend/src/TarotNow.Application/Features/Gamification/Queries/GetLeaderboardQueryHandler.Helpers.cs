using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Helpers;

namespace TarotNow.Application.Features.Gamification.Queries;

public partial class GetLeaderboardQueryHandler
{
    private async Task<LeaderboardResultDto> LoadLeaderboardAsync(LeaderboardScope scope, int limit, CancellationToken ct)
    {
        var snapshot = await _lbRepo.GetSnapshotAsync(scope.Track, scope.PeriodKey, ct);
        if (snapshot != null)
        {
            return new LeaderboardResultDto { Entries = snapshot.Entries, UserRank = null };
        }

        var entries = await _lbRepo.GetTopEntriesAsync(scope.Track, scope.PeriodKey, limit, ct);
        await EnrichEntryProfilesAsync(entries, ct);
        return new LeaderboardResultDto { Entries = entries };
    }

    private async Task EnrichEntryProfilesAsync(List<LeaderboardEntryDto> entries, CancellationToken ct)
    {
        if (entries.Count == 0) return;
        var userIds = entries.Select(e => Guid.Parse(e.UserId)).Distinct();
        var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, ct);
        foreach (var entry in entries)
        {
            if (!userMap.TryGetValue(Guid.Parse(entry.UserId), out var info)) continue;
            entry.DisplayName = info.DisplayName;
            entry.Avatar = info.AvatarUrl;
            entry.ActiveTitle = info.ActiveTitle;
        }
    }

    private async Task PopulateUserRankAsync(LeaderboardResultDto result, Guid userId, LeaderboardScope scope, CancellationToken ct)
    {
        var userRankEntry = await _lbRepo.GetUserRankAsync(userId, scope.Track, scope.PeriodKey, ct);
        if (userRankEntry == null) return;
        var userInfo = await _userRepo.GetByIdAsync(userId, ct);
        result.UserRank = new UserRankDto
        {
            ScoreTrack = scope.Track,
            PeriodKey = scope.PeriodKey,
            Rank = userRankEntry.Rank,
            Score = userRankEntry.Score,
            DisplayName = userInfo?.DisplayName ?? "Người dùng TarotNow",
            Avatar = userInfo?.AvatarUrl,
            ActiveTitle = userInfo?.ActiveTitleRef
        };
    }

    private static LeaderboardScope NormalizeTrack(string? rawTrack, string? rawPeriodKey)
    {
        var track = (rawTrack?.Trim() ?? "spent_gold_daily").ToLowerInvariant();
        var periodKey = rawPeriodKey?.Trim().ToLowerInvariant();
        if (track.EndsWith("_daily", StringComparison.Ordinal))
        {
            return new LeaderboardScope(track[..^6], periodKey ?? PeriodKeyHelper.GetPeriodKey("daily"));
        }

        if (track.EndsWith("_monthly", StringComparison.Ordinal))
        {
            return new LeaderboardScope(track[..^8], periodKey ?? PeriodKeyHelper.GetPeriodKey("monthly"));
        }

        if (track.EndsWith("_all", StringComparison.Ordinal))
        {
            return new LeaderboardScope(track[..^4], periodKey ?? "all");
        }

        return periodKey switch
        {
            "daily" => new LeaderboardScope(track, PeriodKeyHelper.GetPeriodKey("daily")),
            "monthly" => new LeaderboardScope(track, PeriodKeyHelper.GetPeriodKey("monthly")),
            _ => new LeaderboardScope(track, periodKey ?? "all")
        };
    }

    private readonly record struct LeaderboardScope(string Track, string PeriodKey);
}
