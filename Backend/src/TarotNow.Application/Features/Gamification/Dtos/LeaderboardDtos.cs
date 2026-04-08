using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

// DTO snapshot bảng xếp hạng đã chốt.
public class LeaderboardSnapshotDto
{
    // Track điểm dùng để xếp hạng.
    public string ScoreTrack { get; set; } = string.Empty;

    // Khóa chu kỳ snapshot.
    public string PeriodKey { get; set; } = string.Empty;

    // Tổng số người tham gia.
    public int TotalParticipants { get; set; }

    // Danh sách entry của snapshot.
    public List<LeaderboardEntryDto> Entries { get; set; } = new();

    // Thời điểm snapshot được tạo.
    public DateTime CreatedAt { get; set; }
}

// DTO một entry trên bảng xếp hạng.
public class LeaderboardEntryDto
{
    // Định danh user.
    public string UserId { get; set; } = string.Empty;

    // Tên hiển thị.
    public string DisplayName { get; set; } = string.Empty;

    // Avatar user (nếu có).
    public string? Avatar { get; set; }

    // Title đang active (nếu có).
    public string? ActiveTitle { get; set; }

    // Điểm số user.
    public long Score { get; set; }

    // Thứ hạng.
    public int Rank { get; set; }
}

// DTO kết quả truy vấn leaderboard.
public class LeaderboardResultDto
{
    // Danh sách entry top.
    public List<LeaderboardEntryDto> Entries { get; set; } = new();

    // Thông tin thứ hạng của user truy vấn (nếu có).
    public UserRankDto? UserRank { get; set; }
}

// DTO thứ hạng chi tiết của một user.
public class UserRankDto
{
    // Track điểm.
    public string ScoreTrack { get; set; } = string.Empty;

    // Chu kỳ áp dụng.
    public string PeriodKey { get; set; } = string.Empty;

    // Hạng của user.
    public int Rank { get; set; }

    // Điểm của user.
    public long Score { get; set; }

    // Tổng số participant trong bảng.
    public int TotalParticipants { get; set; }

    // Tên hiển thị user.
    public string DisplayName { get; set; } = string.Empty;

    // Avatar user.
    public string? Avatar { get; set; }

    // Title đang active.
    public string? ActiveTitle { get; set; }
}
