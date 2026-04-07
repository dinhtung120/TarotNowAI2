using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community;

public static class CommunityModuleConstants
{
    public static readonly string[] SupportedReactionTypes =
    [
        ReactionType.Like,
        ReactionType.Love,
        ReactionType.Insightful,
        ReactionType.Haha,
        ReactionType.Sad
    ];

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
