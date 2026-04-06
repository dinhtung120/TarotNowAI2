using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

public class LeaderboardSnapshotDto
{
    public string ScoreTrack { get; set; } = string.Empty;
    public string PeriodKey { get; set; } = string.Empty;
    public int TotalParticipants { get; set; }
    public List<LeaderboardEntryDto> Entries { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class LeaderboardEntryDto
{
    public string UserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? ActiveTitle { get; set; }
    public long Score { get; set; }
    public int Rank { get; set; }
}

public class LeaderboardResultDto
{
    public List<LeaderboardEntryDto> Entries { get; set; } = new();
    public UserRankDto? UserRank { get; set; }
}

public class UserRankDto
{
    public string ScoreTrack { get; set; } = string.Empty;
    public string PeriodKey { get; set; } = string.Empty;
    public int Rank { get; set; }
    public long Score { get; set; }
    public int TotalParticipants { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? ActiveTitle { get; set; }
}
