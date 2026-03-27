using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed partial class ChatModerationWorker
{
    private static bool ShouldModerate(string type)
    {
        return string.Equals(type, "text", StringComparison.OrdinalIgnoreCase)
            || string.Equals(type, "system", StringComparison.OrdinalIgnoreCase);
    }

    private static string? MatchKeyword(string content, IReadOnlyCollection<string> keywords)
    {
        if (string.IsNullOrWhiteSpace(content) || keywords.Count == 0)
        {
            return null;
        }

        foreach (var keyword in keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                continue;
            }

            if (content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                return keyword;
            }
        }

        return null;
    }

    private static ReportDto BuildAutoModerationReport(ChatModerationPayload payload, string matchedKeyword)
    {
        return new ReportDto
        {
            ReporterId = Guid.Empty.ToString(),
            TargetType = "message",
            TargetId = payload.MessageId,
            ConversationRef = payload.ConversationId,
            Reason = $"Auto moderation flag: keyword '{matchedKeyword}' detected in chat message.",
            Status = "pending",
            CreatedAt = DateTime.UtcNow,
            AdminNote = $"sender={payload.SenderId}, created_at={payload.CreatedAt:O}"
        };
    }
}
