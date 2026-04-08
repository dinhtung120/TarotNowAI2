using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Helpers;

namespace TarotNow.Application.Features.Gamification.Queries;

public partial class GetLeaderboardQueryHandler
{
    /// <summary>
    /// Tải dữ liệu leaderboard theo phạm vi đã normalize.
    /// Luồng xử lý: ưu tiên snapshot đã chốt; nếu không có thì lấy top entries realtime và enrich profile.
    /// </summary>
    private async Task<LeaderboardResultDto> LoadLeaderboardAsync(LeaderboardScope scope, int limit, CancellationToken ct)
    {
        var snapshot = await _lbRepo.GetSnapshotAsync(scope.Track, scope.PeriodKey, ct);
        if (snapshot != null)
        {
            // Snapshot có sẵn giúp đọc nhanh và nhất quán theo kỳ.
            return new LeaderboardResultDto { Entries = snapshot.Entries, UserRank = null };
        }

        var entries = await _lbRepo.GetTopEntriesAsync(scope.Track, scope.PeriodKey, limit, ct);
        await EnrichEntryProfilesAsync(entries, ct);
        return new LeaderboardResultDto { Entries = entries };
    }

    /// <summary>
    /// Enrich display profile cho các entry leaderboard.
    /// Luồng xử lý: gom user ids, tải map basic info theo batch và gán display name/avatar/active title cho từng entry.
    /// </summary>
    private async Task EnrichEntryProfilesAsync(List<LeaderboardEntryDto> entries, CancellationToken ct)
    {
        if (entries.Count == 0)
        {
            return;
        }

        var userIds = entries.Select(e => Guid.Parse(e.UserId)).Distinct();
        var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, ct);
        foreach (var entry in entries)
        {
            if (!userMap.TryGetValue(Guid.Parse(entry.UserId), out var info))
            {
                // Edge case user không còn tồn tại trong map profile.
                continue;
            }

            entry.DisplayName = info.DisplayName;
            entry.Avatar = info.AvatarUrl;
            entry.ActiveTitle = info.ActiveTitle;
        }
    }

    /// <summary>
    /// Tính và gán thứ hạng riêng của user hiện tại.
    /// Luồng xử lý: lấy rank entry theo user + scope, tải profile user rồi map sang UserRankDto.
    /// </summary>
    private async Task PopulateUserRankAsync(LeaderboardResultDto result, Guid userId, LeaderboardScope scope, CancellationToken ct)
    {
        var userRankEntry = await _lbRepo.GetUserRankAsync(userId, scope.Track, scope.PeriodKey, ct);
        if (userRankEntry == null)
        {
            // User chưa có điểm/hạng trong scope hiện tại.
            return;
        }

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

    /// <summary>
    /// Chuẩn hóa track/period đầu vào về phạm vi leaderboard thống nhất.
    /// Luồng xử lý: hỗ trợ hậu tố _daily/_monthly/_all và fallback theo periodKey truyền vào hoặc mặc định.
    /// </summary>
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

    // Phạm vi leaderboard sau khi normalize.
    private readonly record struct LeaderboardScope(string Track, string PeriodKey);
}
