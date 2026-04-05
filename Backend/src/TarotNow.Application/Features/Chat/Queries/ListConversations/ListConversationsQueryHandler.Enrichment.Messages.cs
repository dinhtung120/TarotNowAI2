using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    private async Task EnrichLastMessagePreviewAsync(
        IEnumerable<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        var conversationIds = conversations.Select(c => c.Id).ToList();
        if (conversationIds.Count == 0) return;

        var latestMessages = await _messageRepo.GetLatestMessagesAsync(conversationIds, cancellationToken);
        var messageMap = latestMessages
            .Where(m => !string.IsNullOrEmpty(m.ConversationId))
            .ToDictionary(m => m.ConversationId, m => m);

        foreach (var conversation in conversations)
        {
            if (messageMap.TryGetValue(conversation.Id, out var lastMessage))
            {
                conversation.LastMessagePreview = BuildPreview(lastMessage);
            }
            else
            {
                conversation.LastMessagePreview = null;
            }
        }
    }

    private static string BuildPreview(ChatMessageDto message)
    {
        if (message.Type == ChatMessageType.CallLog)
        {
            return BuildCallPreview(message);
        }

        var content = ResolveMessageContent(message);
        return ToNormalizedPreview(content);
    }

    private static string ResolveMessageContent(ChatMessageDto message)
    {
        return message.Type switch
        {
            ChatMessageType.Image => "📸 [Hình ảnh]",
            ChatMessageType.Voice => "🎤 [Tin nhắn thoại]",
            ChatMessageType.PaymentOffer => "💳 [Yêu cầu thanh toán]",
            ChatMessageType.PaymentAccept => "✅ [Đã thanh toán]",
            ChatMessageType.PaymentReject => "❌ [Từ chối thanh toán]",
            _ => message.Content
        };

    }

    private static string ToNormalizedPreview(string content)
    {
        var normalized = string.IsNullOrWhiteSpace(content)
            ? "(empty)"
            : content.Trim();
        return normalized.Length <= 90 ? normalized : $"{normalized[..90]}…";
    }

}
