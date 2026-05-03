using System.Collections.Generic;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Hoàn tất conversation sau khi responder chấp thuận và đủ điều kiện chốt phiên.
    /// Luồng xử lý: gọi settle conversation, rồi trả metadata completedAt để client cập nhật timeline.
    /// </summary>
    private async Task<ConversationCompleteRespondResult> CompleteConversationAsync(
        ResponseContext context,
        List<ChatMessageDto> fastLaneMessages,
        CancellationToken cancellationToken)
    {
        var completionMessage = await CompleteAndSettleConversationAsync(
            context.Conversation,
            context.RequesterId,
            "Yêu cầu hoàn thành đã được chấp thuận. Hệ thống đã giải ngân cho Reader.",
            cancellationToken);
        fastLaneMessages.Add(completionMessage);

        return new ConversationCompleteRespondResult
        {
            Status = context.Conversation.Status,
            Accepted = true,
            Metadata = new Dictionary<string, object> { ["completedAt"] = completionMessage.CreatedAt }
        };
    }

    /// <summary>
    /// Chạy settlement và cập nhật conversation sang trạng thái Completed.
    /// Luồng xử lý: settle tài chính trong transaction, thêm system release message, cập nhật state conversation và persist.
    /// </summary>
    private async Task<ChatMessageDto> CompleteAndSettleConversationAsync(
        ConversationDto conversation,
        string actorId,
        string completionMessage,
        CancellationToken cancellationToken)
    {
        await _transactionCoordinator.ExecuteAsync(
            transactionCt => SettleConversationSessionAsync(conversation.Id, transactionCt),
            cancellationToken);

        var releaseMessage = await AddSystemMessageAsync(
            conversation,
            actorId,
            new SystemMessageSpec(ChatMessageType.SystemRelease, completionMessage, DateTime.UtcNow),
            cancellationToken);

        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.Confirm!.AutoResolveAt = null;
        conversation.UpdatedAt = releaseMessage.CreatedAt;
        conversation.LastMessageAt = releaseMessage.CreatedAt;
        // Đồng bộ trạng thái cuối cùng ngay sau settlement để tránh hiển thị pending giả.
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
        return releaseMessage;
    }

    /// <summary>
    /// Settle phiên tài chính liên kết với conversation hiện tại.
    /// Luồng xử lý: tải session theo conversation, giải ngân gộp theo session cho item Accepted rồi lưu thay đổi.
    /// </summary>
    private async Task SettleConversationSessionAsync(string conversationId, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionByConversationRefAsync(conversationId, cancellationToken);
        if (session == null)
        {
            // Edge case: không có session tài chính thì bỏ qua settlement.
            return;
        }

        var items = await _financeRepository.GetItemsBySessionIdAsync(session.Id, cancellationToken);
        await _escrowSettlementService.ApplySessionReleaseAsync(
            session,
            items,
            isAutoRelease: false,
            cancellationToken);

        // Persist settlement session-level trong cùng transaction.
        await _financeRepository.SaveChangesAsync(cancellationToken);
    }
}
