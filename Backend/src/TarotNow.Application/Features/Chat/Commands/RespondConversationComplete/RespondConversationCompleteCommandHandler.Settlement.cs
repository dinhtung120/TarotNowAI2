using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandExecutor
{
    /// <summary>
    /// Hoàn tất conversation sau khi responder chấp thuận và đủ điều kiện chốt phiên.
    /// Luồng xử lý: gọi settle conversation, rồi trả metadata completedAt để client cập nhật timeline.
    /// </summary>
    private async Task<ConversationCompleteRespondResult> CompleteConversationAsync(
        ResponseContext context,
        CancellationToken cancellationToken)
    {
        var completedAt = await CompleteAndSettleConversationAsync(
            context.Conversation,
            context.RequesterId,
            "Yêu cầu hoàn thành đã được chấp thuận. Hệ thống đã giải ngân cho Reader.",
            cancellationToken);

        return new ConversationCompleteRespondResult
        {
            Status = context.Conversation.Status,
            Accepted = true,
            Metadata = new Dictionary<string, object> { ["completedAt"] = completedAt }
        };
    }

    /// <summary>
    /// Chạy settlement và cập nhật conversation sang trạng thái Completed.
    /// Luồng xử lý: settle tài chính trong transaction, thêm system release message, cập nhật state conversation và persist.
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
        // Đồng bộ trạng thái cuối cùng ngay sau settlement để tránh hiển thị pending giả.
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
        return completedAt;
    }

    /// <summary>
    /// Settle phiên tài chính liên kết với conversation hiện tại.
    /// Luồng xử lý: tải session theo conversation, release item Accepted, lưu thay đổi và đóng session khi không còn frozen.
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
        foreach (var item in items.Where(item => item.Status == QuestionItemStatus.Accepted))
        {
            // Business rule: chỉ release item đã Accepted để đảm bảo đúng quyền lợi thanh toán.
            await _escrowSettlementService.ApplyReleaseAsync(item, isAutoRelease: false, cancellationToken);
        }

        await _financeRepository.SaveChangesAsync(cancellationToken);
        // Sau khi release item, kiểm tra lại frozen để quyết định đóng session.
        await MarkCompletedSessionWhenNoFrozenAsync(session.Id, cancellationToken);
    }

    /// <summary>
    /// Đánh dấu session completed khi không còn số dư bị khóa.
    /// Luồng xử lý: khóa session để cập nhật nhất quán, kiểm tra TotalFrozen, cập nhật trạng thái và thời điểm cập nhật.
    /// </summary>
    private async Task MarkCompletedSessionWhenNoFrozenAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var lockedSession = await _financeRepository.GetSessionForUpdateAsync(sessionId, cancellationToken);
        if (lockedSession == null)
        {
            // Edge case: session không còn tồn tại tại thời điểm cập nhật.
            return;
        }

        if (lockedSession.TotalFrozen <= 0)
        {
            // Chỉ đóng session khi frozen đã về 0 để đúng nghiệp vụ đối soát.
            lockedSession.Status = ChatFinanceSessionStatus.Completed;
        }

        lockedSession.UpdatedAt = DateTime.UtcNow;
        await _financeRepository.UpdateSessionAsync(lockedSession, cancellationToken);
        // Persist thay đổi trạng thái session để hoàn tất vòng đời phiên.
        await _financeRepository.SaveChangesAsync(cancellationToken);
    }
}
