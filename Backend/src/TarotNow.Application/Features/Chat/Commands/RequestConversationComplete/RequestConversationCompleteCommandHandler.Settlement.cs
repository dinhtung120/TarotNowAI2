using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandler
{
    /// <summary>
    /// Hoàn tất conversation khi đã đủ xác nhận từ hai phía.
    /// Luồng xử lý: gọi settle nghiệp vụ tài chính, rồi trả metadata completedAt cho client đồng bộ trạng thái.
    /// </summary>
    private async Task<ConversationActionResult> CompleteConversationAsync(
        RequestContext context,
        CancellationToken cancellationToken)
    {
        var completedAt = await CompleteAndSettleConversationAsync(
            context.Conversation,
            context.RequesterId,
            "Hai bên đã xác nhận hoàn thành. Hệ thống đã giải ngân cho Reader.",
            cancellationToken);

        return new ConversationActionResult
        {
            Status = context.Conversation.Status,
            Metadata = new Dictionary<string, object> { ["completedAt"] = completedAt }
        };
    }

    /// <summary>
    /// Thực hiện settle và chuyển trạng thái conversation sang Completed.
    /// Luồng xử lý: chạy settlement trong transaction, ghi system release message, cập nhật state conversation và persist.
    /// </summary>
    private async Task<DateTime> CompleteAndSettleConversationAsync(
        ConversationDto conversation,
        string actorId,
        string completionMessage,
        CancellationToken cancellationToken)
    {
        await _transactionCoordinator.ExecuteAsync(
            transactionCt => SettleConversationSessionAsync(conversation.Id, transactionCt),
            cancellationToken);

        var completedAt = await AddSystemMessageAsync(
            conversation,
            actorId,
            new SystemMessageSpec(ChatMessageType.SystemRelease, completionMessage, DateTime.UtcNow),
            cancellationToken);

        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.Confirm!.AutoResolveAt = null;
        conversation.UpdatedAt = completedAt;
        conversation.LastMessageAt = completedAt;
        // Đồng bộ state conversation ngay sau khi settlement thành công để tránh pending giả trên UI.
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
        return completedAt;
    }

    /// <summary>
    /// Settle toàn bộ phiên tài chính gắn với conversation.
    /// Luồng xử lý: tìm session theo conversation, release các item đã Accepted, rồi đóng session khi không còn frozen.
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
        // Chỉ release các item đã Accepted để không trả tiền cho item bị từ chối/chưa xử lý.
        await ReleaseAcceptedItemsAsync(items, cancellationToken);
        await MarkCompletedSessionWhenNoFrozenAsync(session.Id, cancellationToken);
    }

    /// <summary>
    /// Giải ngân các item đã được chấp nhận trong phiên.
    /// Luồng xử lý: lặp qua item Accepted, gọi escrow settlement, rồi flush thay đổi finance.
    /// </summary>
    private async Task ReleaseAcceptedItemsAsync(
        IEnumerable<Domain.Entities.ChatQuestionItem> items,
        CancellationToken cancellationToken)
    {
        foreach (var item in items.Where(item => item.Status == QuestionItemStatus.Accepted))
        {
            // Business rule: chỉ item Accepted mới được phép release escrow.
            await _escrowSettlementService.ApplyReleaseAsync(item, isAutoRelease: false, cancellationToken);
        }

        // Lưu một lần sau vòng lặp để giảm số lần ghi DB.
        await _financeRepository.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Đánh dấu phiên completed khi đã không còn số dư bị khóa.
    /// Luồng xử lý: khóa session để cập nhật an toàn, kiểm tra TotalFrozen, cập nhật trạng thái và thời gian chỉnh sửa.
    /// </summary>
    private async Task MarkCompletedSessionWhenNoFrozenAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var lockedSession = await _financeRepository.GetSessionForUpdateAsync(sessionId, cancellationToken);
        if (lockedSession == null)
        {
            // Edge case: session bị xóa/không còn khả dụng khi vào nhánh cập nhật.
            return;
        }

        if (lockedSession.TotalFrozen <= 0)
        {
            // Chỉ đóng session khi đã giải ngân hết để không sai lệch đối soát.
            lockedSession.Status = "completed";
        }

        lockedSession.UpdatedAt = DateTime.UtcNow;
        await _financeRepository.UpdateSessionAsync(lockedSession, cancellationToken);
        // Persist thay đổi trạng thái session trong cùng luồng nghiệp vụ.
        await _financeRepository.SaveChangesAsync(cancellationToken);
    }
}
