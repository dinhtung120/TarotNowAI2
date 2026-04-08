using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

// DTO định nghĩa quest.
public class QuestDefinitionDto
{
    // Mã quest.
    public string Code { get; set; } = string.Empty;

    // Tiêu đề quest tiếng Việt.
    public string TitleVi { get; set; } = string.Empty;

    // Tiêu đề quest tiếng Anh.
    public string TitleEn { get; set; } = string.Empty;

    // Tiêu đề quest tiếng Trung.
    public string TitleZh { get; set; } = string.Empty;

    // Mô tả quest tiếng Việt.
    public string DescriptionVi { get; set; } = string.Empty;

    // Mô tả quest tiếng Anh.
    public string DescriptionEn { get; set; } = string.Empty;

    // Mô tả quest tiếng Trung.
    public string DescriptionZh { get; set; } = string.Empty;

    // Loại quest (daily/monthly/...).
    public string QuestType { get; set; } = string.Empty;

    // Sự kiện kích hoạt tiến độ quest.
    public string TriggerEvent { get; set; } = string.Empty;

    // Mục tiêu tiến độ cần đạt.
    public int Target { get; set; }

    // Danh sách phần thưởng của quest.
    public List<QuestRewardItemDto> Rewards { get; set; } = new();

    // Cờ active/inactive của quest.
    public bool IsActive { get; set; }
}

// DTO một phần thưởng trong quest.
public class QuestRewardItemDto
{
    // Loại phần thưởng (gold/diamond/title).
    public string Type { get; set; } = string.Empty;

    // Giá trị số lượng phần thưởng.
    public int Amount { get; set; }

    // Mã title thưởng kèm (nếu reward type là title).
    public string? TitleCode { get; set; }
}

// DTO tiến độ quest của user.
public class QuestProgressDto
{
    // Định danh user.
    public string UserId { get; set; } = string.Empty;

    // Mã quest.
    public string QuestCode { get; set; } = string.Empty;

    // Chu kỳ tiến độ quest.
    public string PeriodKey { get; set; } = string.Empty;

    // Tiến độ hiện tại.
    public int CurrentProgress { get; set; }

    // Mục tiêu cần đạt.
    public int Target { get; set; }

    // Cờ đã claim thưởng hay chưa.
    public bool IsClaimed { get; set; }

    // Thời điểm đã claim (nếu có).
    public DateTime? ClaimedAt { get; set; }
}

// DTO ghép định nghĩa quest với tiến độ user.
public class QuestWithProgressDto
{
    // Định nghĩa quest.
    public QuestDefinitionDto Definition { get; set; } = new();

    // Tiến độ tương ứng của user (nếu có).
    public QuestProgressDto? Progress { get; set; }
}

// DTO kết quả claim reward quest.
public class ClaimQuestRewardResult
{
    // Cờ claim thành công.
    public bool Success { get; set; }

    // Cờ quest đã được claim trước đó.
    public bool AlreadyClaimed { get; set; }

    // Loại phần thưởng trả về chính.
    public string RewardType { get; set; } = string.Empty;

    // Số lượng phần thưởng trả về chính.
    public int RewardAmount { get; set; }
}
