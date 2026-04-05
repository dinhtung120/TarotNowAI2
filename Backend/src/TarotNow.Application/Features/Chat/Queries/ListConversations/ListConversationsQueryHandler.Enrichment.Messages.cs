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
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(message.Content);
                var callTypeRaw = doc.RootElement.TryGetProperty("CallType", out var cProp) ? cProp.GetString() : 
                                  (doc.RootElement.TryGetProperty("callType", out var cProp2) ? cProp2.GetString() : "audio");
                var duration = doc.RootElement.TryGetProperty("DurationSeconds", out var dProp) && dProp.TryGetInt32(out var d) ? d : 
                               (doc.RootElement.TryGetProperty("durationSeconds", out var dProp2) && dProp2.TryGetInt32(out var d2) ? d2 : 0);

                var typeStr = callTypeRaw == "video" ? "Cuộc gọi video" : "Cuộc gọi thoại";
                var icon = callTypeRaw == "video" ? "🎥" : "📞";

                if (duration == 0) return $"{icon} {typeStr} bị nhỡ";
                
                var min = duration / 60;
                var sec = duration % 60;
                return $"{icon} {typeStr} ({min:00}:{sec:00})";
            }
            catch
            {
                return "📞 Cuộc gọi";
            }
        }

        var content = message.Type switch
        {
            ChatMessageType.Image => "📸 [Hình ảnh]",
            ChatMessageType.Voice => "🎤 [Tin nhắn thoại]",
            ChatMessageType.PaymentOffer => "💳 [Yêu cầu thanh toán]",
            ChatMessageType.PaymentAccept => "✅ [Đã thanh toán]",
            ChatMessageType.PaymentReject => "❌ [Từ chối thanh toán]",
            _ => message.Content
        };

        var normalized = string.IsNullOrWhiteSpace(content)
            ? "(empty)"
            : content.Trim();

        return normalized.Length <= 90 ? normalized : $"{normalized[..90]}…";
    }
}
