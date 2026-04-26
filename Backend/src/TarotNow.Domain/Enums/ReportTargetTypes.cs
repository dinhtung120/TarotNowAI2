namespace TarotNow.Domain.Enums;

/// <summary>
/// Danh mục target type cho feature report/moderation.
/// </summary>
public static class ReportTargetTypes
{
    public const string Message = "message";
    public const string Conversation = "conversation";
    public const string User = "user";

    public static readonly string[] All = [Message, Conversation, User];
}
