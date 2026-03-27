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
        var content = message.Type switch
        {
            ChatMessageType.Image => "[Image]",
            ChatMessageType.Voice => "[Voice]",
            ChatMessageType.PaymentOffer => "[Payment Request]",
            ChatMessageType.PaymentAccept => "[Payment Accepted]",
            ChatMessageType.PaymentReject => "[Payment Rejected]",
            _ => message.Content
        };

        var normalized = string.IsNullOrWhiteSpace(content)
            ? "(empty)"
            : content.Trim();

        return normalized.Length <= 90 ? normalized : $"{normalized[..90]}…";
    }
}
