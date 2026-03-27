namespace TarotNow.Application.Common;

public class ChatModerationPayload
{
    public string MessageId { get; set; } = string.Empty;

    public string ConversationId { get; set; } = string.Empty;

    public string SenderId { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
