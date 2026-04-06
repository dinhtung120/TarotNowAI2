using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

public class QuestDefinitionDto
{
    public string Code { get; set; } = string.Empty;
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string TitleZh { get; set; } = string.Empty;
    public string DescriptionVi { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionZh { get; set; } = string.Empty;
    public string QuestType { get; set; } = string.Empty;
    public string TriggerEvent { get; set; } = string.Empty;
    public int Target { get; set; }
    public List<QuestRewardItemDto> Rewards { get; set; } = new();
    public bool IsActive { get; set; }
}

public class QuestRewardItemDto
{
    public string Type { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string? TitleCode { get; set; }
}

public class QuestProgressDto
{
    public string UserId { get; set; } = string.Empty;
    public string QuestCode { get; set; } = string.Empty;
    public string PeriodKey { get; set; } = string.Empty;
    public int CurrentProgress { get; set; }
    public int Target { get; set; }
    public bool IsClaimed { get; set; }
    public DateTime? ClaimedAt { get; set; }
}

public class QuestWithProgressDto
{
    public QuestDefinitionDto Definition { get; set; } = new();
    public QuestProgressDto? Progress { get; set; }
}

public class ClaimQuestRewardResult
{
    public bool Success { get; set; }
    public bool AlreadyClaimed { get; set; }
    public string RewardType { get; set; } = string.Empty;
    public int RewardAmount { get; set; }
}
