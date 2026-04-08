using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community;

// Hằng số dùng chung cho nghiệp vụ module Community.
public static class CommunityModuleConstants
{
    // Danh sách reaction type được hệ thống hỗ trợ.
    public static readonly string[] SupportedReactionTypes =
    [
        ReactionType.Like,
        ReactionType.Love,
        ReactionType.Insightful,
        ReactionType.Haha,
        ReactionType.Sad
    ];

    // Danh sách reason code hợp lệ cho report community post.
    public static readonly string[] SupportedReportReasonCodes =
    [
        "spam",
        "hate_speech",
        "harassment",
        "misinformation",
        "inappropriate",
        "other"
    ];
}
