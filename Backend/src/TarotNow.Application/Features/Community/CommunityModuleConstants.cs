using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community;

/// <summary>
/// Centralized constants for Community feature to avoid magic strings across handlers.
/// </summary>
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
