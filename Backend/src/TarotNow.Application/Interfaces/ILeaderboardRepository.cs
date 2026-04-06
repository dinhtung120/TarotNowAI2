using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

public interface ILeaderboardRepository
{
    Task<LeaderboardSnapshotDto?> GetSnapshotAsync(string scoreTrack, string periodKey, CancellationToken ct);
    Task UpsertSnapshotAsync(LeaderboardSnapshotDto snapshot, CancellationToken ct);
    
    Task IncrementScoreAsync(Guid userId, string scoreTrack, string periodKey, long points, CancellationToken ct);
    Task<List<LeaderboardEntryDto>> GetTopEntriesAsync(string scoreTrack, string periodKey, int limit, CancellationToken ct);
    Task<LeaderboardEntryDto?> GetUserRankAsync(Guid userId, string scoreTrack, string periodKey, CancellationToken ct);
    
    Task ResetScoreTrackAsync(string scoreTrack, string periodKey, CancellationToken ct);
}
