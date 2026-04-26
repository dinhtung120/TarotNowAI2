using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public partial class RejectConversationCommandExecutor
{
    /// <summary>
    /// Tải conversation và kiểm tra điều kiện hợp lệ để reader reject.
    /// Luồng xử lý: kiểm tra conversation tồn tại, reader ownership và trạng thái pending/awaiting acceptance.
    /// </summary>
    private async Task<ConversationDto> LoadConversationForRejectAsync(
        RejectConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.ReaderId != request.ReaderId.ToString())
        {
            // Chặn reader không thuộc conversation thực hiện reject.
            throw new BadRequestException("Bạn không thể reject cuộc trò chuyện này.");
        }

        if (conversation.Status is not (ConversationStatus.Pending or ConversationStatus.AwaitingAcceptance))
        {
            // Chỉ cho reject ở trạng thái chưa đi vào xử lý chính thức.
            throw new BadRequestException($"Không thể reject ở trạng thái '{conversation.Status}'.");
        }

        return conversation;
    }

    /// <summary>
    /// Xác định item có đủ điều kiện hoàn tiền trước khi reject hay không.
    /// Luồng xử lý: chấp nhận item pending hoặc accepted nhưng chưa có repliedAt.
    /// </summary>
    private static bool IsRefundableBeforeReject(Domain.Entities.ChatQuestionItem item)
    {
        return item.Status == QuestionItemStatus.Pending
               || (item.Status == QuestionItemStatus.Accepted && item.RepliedAt == null);
    }

    /// <summary>
    /// Thêm system message thông báo reject + hoàn tiền nếu có refund.
    /// Luồng xử lý: bỏ qua khi refundedAmount <= 0, chuẩn hóa reason và ghi message system refund.
    /// </summary>
    private async Task TryAppendRejectSystemMessageAsync(
        ConversationDto conversation,
        string? reason,
        long refundedAmount,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (refundedAmount <= 0)
        {
            // Không hoàn tiền thì không cần phát system message refund.
            return;
        }

        // Chuẩn hóa reason để hiển thị rõ ràng trong nội dung message hệ thống.
        var rejectReason = string.IsNullOrWhiteSpace(reason) ? "Không có" : reason.Trim();
        var systemMessage = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.SystemRefund,
            Content = $"Reader đã từ chối câu hỏi. Lý do: {rejectReason}. Đã hoàn {refundedAmount} 💎 về ví của bạn.",
            IsRead = false,
            CreatedAt = now
        };

        await _chatMessageRepository.AddAsync(systemMessage, cancellationToken);
        conversation.LastMessageAt = systemMessage.CreatedAt;
    }
}
