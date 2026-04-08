using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed partial class ChatModerationWorker
{
    /// <summary>
    /// Quyết định loại tin nhắn có cần đưa vào luồng kiểm duyệt hay không.
    /// Luồng xử lý: so sánh type với nhóm text/system không phân biệt hoa thường.
    /// </summary>
    private static bool ShouldModerate(string type)
    {
        return string.Equals(type, "text", StringComparison.OrdinalIgnoreCase)
            || string.Equals(type, "system", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Tìm keyword vi phạm đầu tiên khớp trong nội dung tin nhắn.
    /// Luồng xử lý: validate input, duyệt lần lượt keyword và trả keyword đầu tiên khớp; không khớp thì null.
    /// </summary>
    private static string? MatchKeyword(string content, IReadOnlyCollection<string> keywords)
    {
        if (string.IsNullOrWhiteSpace(content) || keywords.Count == 0)
        {
            // Edge case: thiếu nội dung hoặc danh sách keyword rỗng thì không thể xác định vi phạm.
            return null;
        }

        foreach (var keyword in keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Bỏ qua keyword rỗng để tránh match giả.
                continue;
            }

            if (content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                // Trả keyword đầu tiên để giảm chi phí quét và log rõ nguyên nhân flag.
                return keyword;
            }
        }

        return null;
    }

    /// <summary>
    /// Dựng report tự động cho tin nhắn bị phát hiện keyword vi phạm.
    /// Luồng xử lý: map dữ liệu payload + keyword thành ReportDto chuẩn để ghi vào moderation queue.
    /// </summary>
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
