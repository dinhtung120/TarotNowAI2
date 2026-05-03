using System.Collections.Generic;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Hoàn tất conversation khi đã đủ xác nhận từ hai phía.
    /// Luồng xử lý: gọi settle nghiệp vụ tài chính, rồi trả metadata completedAt cho client đồng bộ trạng thái.
    /// </summary>
    private async Task<ConversationActionResult> CompleteConversationAsync(
        RequestContext context,
        List<ChatMessageDto> fastLaneMessages,
        CancellationToken cancellationToken)
    {
        var completionMessage = await CompleteAndSettleConversationAsync(
            context.Conversation,
            context.RequesterId,
            "Hai bên đã xác nhận hoàn thành. Hệ thống đã giải ngân cho Reader.",
            cancellationToken);
        fastLaneMessages.Add(completionMessage);

        return new ConversationActionResult
        {
            Status = context.Conversation.Status,
            Metadata = new Dictionary<string, object> { ["completedAt"] = completionMessage.CreatedAt }
        };
    }

    /// <summary>
    /// Thực hiện settle và chuyển trạng thái conversation sang Completed.
    /// Luồng xử lý: chạy settlement trong transaction, ghi system release message, cập nhật state conversation và persist.
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
        // Đồng bộ state conversation ngay sau khi settlement thành công để tránh pending giả trên UI.
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
        return releaseMessage;
    }

    /// <summary>
    /// Settle toàn bộ phiên tài chính gắn với conversation.
    /// Luồng xử lý: tìm session theo conversation, giải ngân gộp theo session cho các item Accepted rồi lưu thay đổi.
    /// </summary>
    private async Task SettleConversationSessionAsync(string conversationId, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionByConversationRefAsync(conversationId, cancellationToken);
        if (session == null)
        {
            // Edge case: conversation chưa tạo phiên tài chính thì bỏ qua settlement.
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
