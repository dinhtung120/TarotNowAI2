using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

public class AchievementDefinitionDto
{
    public string Code { get; set; } = string.Empty;
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string TitleZh { get; set; } = string.Empty;
    public string DescriptionVi { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionZh { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? GrantsTitleCode { get; set; }
    public bool IsActive { get; set; }
}

public class UserAchievementDto
{
    public string AchievementCode { get; set; } = string.Empty;
    public DateTime UnlockedAt { get; set; }
}

public class UserAchievementsDto
{
    public List<AchievementDefinitionDto> Definitions { get; set; } = new();
    public List<UserAchievementDto> UnlockedList { get; set; } = new();
}
