using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private async Task TryMarkReaderRepliedAsync(
        ConversationDto conversation,
        string senderId,
        string messageType,
        CancellationToken cancellationToken)
    {
        if (ShouldSkipReaderReplyMark(conversation, senderId, messageType))
        {
            return;
        }

        await _transactionCoordinator.ExecuteAsync(
            transactionCt => MarkAcceptedItemsAsRepliedAsync(conversation.Id, transactionCt),
            cancellationToken);
    }

    private static bool ShouldSkipReaderReplyMark(
        ConversationDto conversation,
        string senderId,
        string messageType)
    {
        return conversation.Status != ConversationStatus.Ongoing
               || senderId != conversation.ReaderId
               || IsReaderReplySignalMessageType(messageType) == false;
    }

    private async Task MarkAcceptedItemsAsRepliedAsync(
        string conversationId,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(conversationId, cancellationToken);
        if (session == null)
        {
            return;
        }

        var items = await _financeRepo.GetItemsBySessionIdAsync(session.Id, cancellationToken);
        var candidates = items
            .Where(item => item.Status == QuestionItemStatus.Accepted && item.RepliedAt == null)
            .ToList();

        if (candidates.Count == 0)
        {
            return;
        }

        var now = DateTime.UtcNow;
        foreach (var item in candidates)
        {
            item.RepliedAt = now;
            item.AutoReleaseAt = now.AddHours(24);
            item.UpdatedAt = now;
            await _financeRepo.UpdateItemAsync(item, cancellationToken);
        }

        await _financeRepo.SaveChangesAsync(cancellationToken);
    }

    private static bool IsReaderReplySignalMessageType(string type)
    {
        return type is ChatMessageType.Text
            or ChatMessageType.Image
            or ChatMessageType.Voice
            or ChatMessageType.CardShare;
    }
}
