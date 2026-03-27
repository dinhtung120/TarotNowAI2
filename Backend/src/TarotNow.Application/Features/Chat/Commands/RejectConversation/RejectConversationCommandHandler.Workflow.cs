using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public partial class RejectConversationCommandHandler
{
    private async Task<ConversationDto> LoadConversationForRejectAsync(
        RejectConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.ReaderId != request.ReaderId.ToString())
        {
            throw new BadRequestException("Bạn không thể reject cuộc trò chuyện này.");
        }

        if (conversation.Status is not (ConversationStatus.Pending or ConversationStatus.AwaitingAcceptance))
        {
            throw new BadRequestException($"Không thể reject ở trạng thái '{conversation.Status}'.");
        }

        return conversation;
    }

    private static bool IsRefundableBeforeReject(Domain.Entities.ChatQuestionItem item)
    {
        return item.Status == QuestionItemStatus.Pending
               || (item.Status == QuestionItemStatus.Accepted && item.RepliedAt == null);
    }

    private async Task TryAppendRejectSystemMessageAsync(
        ConversationDto conversation,
        string? reason,
        long refundedAmount,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (refundedAmount <= 0)
        {
            return;
        }

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
