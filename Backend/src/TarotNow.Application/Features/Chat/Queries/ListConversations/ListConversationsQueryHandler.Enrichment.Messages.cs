using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    /// <summary>
    /// Enrich preview tin nhắn cuối cùng cho danh sách conversation.
    /// Luồng xử lý: lấy id conversation, truy vấn latest message theo lô, map theo conversation id và gán preview tương ứng.
    /// </summary>
    private async Task EnrichLastMessagePreviewAsync(
        IEnumerable<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        var conversationIds = conversations.Select(c => c.Id).ToList();
        if (conversationIds.Count == 0)
        {
            return;
        }

        var latestMessages = await _messageRepo.GetLatestMessagesAsync(conversationIds, cancellationToken);
        var messageMap = latestMessages
            // Bỏ qua bản ghi message thiếu ConversationId để tránh key rỗng trong dictionary.
            .Where(m => !string.IsNullOrEmpty(m.ConversationId))
            .ToDictionary(m => m.ConversationId, m => m);

        foreach (var conversation in conversations)
        {
            if (messageMap.TryGetValue(conversation.Id, out var lastMessage))
            {
                // Có message cuối thì dựng preview theo loại message.
                conversation.LastMessagePreview = BuildPreview(lastMessage);
            }
            else
            {
                // Edge case conversation chưa có message thì giữ preview null.
                conversation.LastMessagePreview = null;
            }
        }
    }

    /// <summary>
    /// Dựng preview text cho một message.
    /// Luồng xử lý: call-log đi qua formatter riêng; các loại khác chuyển content về chuỗi preview chuẩn hóa.
    /// </summary>
    private static string BuildPreview(ChatMessageDto message)
    {
        if (message.Type == ChatMessageType.CallLog)
        {
            return BuildCallPreview(message);
        }

        var content = ResolveMessageContent(message);
        return ToNormalizedPreview(content);
    }

    /// <summary>
    /// Chuyển message về nội dung hiển thị preview theo từng loại.
    /// Luồng xử lý: map các loại media/payment sang nhãn dễ đọc; mặc định dùng content gốc.
    /// </summary>
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

    /// <summary>
    /// Chuẩn hóa chuỗi preview cuối cùng.
    /// Luồng xử lý: thay nội dung rỗng bằng "(empty)", trim khoảng trắng và cắt tối đa 90 ký tự.
    /// </summary>
    private static string ToNormalizedPreview(string content)
    {
        var normalized = string.IsNullOrWhiteSpace(content)
            ? "(empty)"
            : content.Trim();
        return normalized.Length <= 90 ? normalized : $"{normalized[..90]}…";
    }
}
