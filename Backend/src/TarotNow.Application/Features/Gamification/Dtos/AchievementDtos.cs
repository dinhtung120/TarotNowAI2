using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

// DTO định nghĩa achievement trong hệ thống.
public class AchievementDefinitionDto
{
    // Mã achievement.
    public string Code { get; set; } = string.Empty;

    // Tiêu đề tiếng Việt.
    public string TitleVi { get; set; } = string.Empty;

    // Tiêu đề tiếng Anh.
    public string TitleEn { get; set; } = string.Empty;

    // Tiêu đề tiếng Trung.
    public string TitleZh { get; set; } = string.Empty;

    // Mô tả tiếng Việt.
    public string DescriptionVi { get; set; } = string.Empty;

    // Mô tả tiếng Anh.
    public string DescriptionEn { get; set; } = string.Empty;

    // Mô tả tiếng Trung.
    public string DescriptionZh { get; set; } = string.Empty;

    // Icon hiển thị achievement (nếu có).
    public string? Icon { get; set; }

    // Title được mở khóa kèm achievement (nếu có).
    public string? GrantsTitleCode { get; set; }

    // Cờ active/inactive của achievement.
    public bool IsActive { get; set; }
}

// DTO một achievement user đã mở khóa.
public class UserAchievementDto
{
    // Mã achievement đã mở khóa.
    public string AchievementCode { get; set; } = string.Empty;

    // Thời điểm mở khóa.
    public DateTime UnlockedAt { get; set; }
}

// DTO tổng hợp danh sách achievement và trạng thái mở khóa của user.
public class UserAchievementsDto
{
    // Danh sách định nghĩa achievement.
    public List<AchievementDefinitionDto> Definitions { get; set; } = new();

    // Danh sách achievement user đã mở khóa.
    public List<UserAchievementDto> UnlockedList { get; set; } = new();
}
