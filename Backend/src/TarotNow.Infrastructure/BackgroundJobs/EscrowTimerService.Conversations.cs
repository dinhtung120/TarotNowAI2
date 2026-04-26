using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Đánh dấu finance session refunded khi không còn frozen balance.
    /// Luồng xử lý: lock session for update, kiểm tra TotalFrozen, set status refunded và persist.
    /// </summary>
    private static async Task MarkSessionRefundedWhenFullyReleasedAsync(
        IChatFinanceRepository financeRepository,
        Guid financeSessionId,
        CancellationToken cancellationToken)
    {
        var session = await financeRepository.GetSessionForUpdateAsync(financeSessionId, cancellationToken);
        if (session == null || session.TotalFrozen > 0)
        {
            // Session chưa tồn tại hoặc còn frozen thì chưa thể chốt refunded.
            return;
        }

        session.Status = ChatFinanceSessionStatus.Refunded;
        session.UpdatedAt = DateTime.UtcNow;
        await financeRepository.UpdateSessionAsync(session, cancellationToken);
        // Đồng bộ trạng thái session để nghiệp vụ phía conversation đọc đúng kết quả.
    }

    /// <summary>
    /// Đánh dấu conversation expired và tạo system message hoàn tiền.
    /// Luồng xử lý: tải conversation, kiểm tra trạng thái hợp lệ, thêm message hệ thống rồi cập nhật trạng thái/timestamp.
    /// </summary>
    private static async Task MarkConversationExpiredAsync(
        RefundDependencies dependencies,
        string conversationId,
        string systemMessage,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null)
        {
            // Conversation có thể đã bị xóa hoặc không còn truy xuất được.
            return;
        }

        if (ConversationStatus.IsTerminal(conversation.Status) || conversation.Status == ConversationStatus.Disputed)
        {
            // Chỉ update conversation còn mở; tránh ghi đè các trạng thái terminal/disputed.
            return;
        }

        var now = DateTime.UtcNow;
        var message = new ChatMessageDto
        {
            ConversationId = conversationId,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.SystemRefund,
            Content = systemMessage,
            IsRead = false,
            CreatedAt = now
        };
        await dependencies.MessageRepository.AddAsync(message, cancellationToken);
        // Gửi system message trước để đảm bảo timeline chat phản ánh lý do expired.

        conversation.Status = ConversationStatus.Expired;
        conversation.OfferExpiresAt = null;
        conversation.LastMessageAt = now;
        conversation.UpdatedAt = now;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }

    /// <summary>
    /// Đánh dấu conversation completed và tạo system message giải ngân.
    /// Luồng xử lý: kiểm tra trạng thái conversation cho phép chuyển completed, thêm message hệ thống, cập nhật metadata.
    /// </summary>
    private static async Task MarkConversationCompletedAsync(
        RefundDependencies dependencies,
        string conversationId,
        string systemMessage,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null)
        {
            // Conversation không tồn tại thì bỏ qua để tránh ném lỗi nền.
            return;
        }

        if (conversation.Status is not (ConversationStatus.Ongoing or ConversationStatus.AwaitingAcceptance or ConversationStatus.Disputed))
        {
            // Chỉ các trạng thái chuyển tiếp hợp lệ mới được chốt completed.
            return;
        }

        var now = DateTime.UtcNow;
        var message = new ChatMessageDto
        {
            ConversationId = conversationId,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.SystemRelease,
            Content = systemMessage,
            IsRead = false,
            CreatedAt = now
        };
        await dependencies.MessageRepository.AddAsync(message, cancellationToken);
        // Tạo system message để người dùng biết conversation đã được chốt tự động.

        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.LastMessageAt = now;
        conversation.UpdatedAt = now;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
