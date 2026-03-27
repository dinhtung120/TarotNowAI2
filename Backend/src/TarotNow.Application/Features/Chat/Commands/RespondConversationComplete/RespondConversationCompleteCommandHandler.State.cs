using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandler
{
    private readonly record struct ResponseContext(
        ConversationDto Conversation,
        string RequesterId,
        DateTime Now);

    private async Task<ResponseContext> BuildContextAsync(
        RespondConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            throw new BadRequestException("Bạn không thuộc cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            throw new BadRequestException($"Không thể phản hồi hoàn thành ở trạng thái '{conversation.Status}'.");
        }

        if (conversation.Confirm == null || string.IsNullOrWhiteSpace(conversation.Confirm.RequestedBy))
        {
            throw new BadRequestException("Không có yêu cầu hoàn thành nào đang chờ phản hồi.");
        }

        if (string.Equals(conversation.Confirm.RequestedBy, requesterId, StringComparison.Ordinal))
        {
            throw new BadRequestException("Bạn là người đã gửi yêu cầu hoàn thành, không thể tự phản hồi.");
        }

        return new ResponseContext(conversation, requesterId, DateTime.UtcNow);
    }

    private static void ApplyResponderConfirmation(ResponseContext context)
    {
        if (context.Conversation.UserId == context.RequesterId)
        {
            context.Conversation.Confirm!.UserAt = context.Now;
            return;
        }

        context.Conversation.Confirm!.ReaderAt = context.Now;
    }

    private static bool HasBothSidesConfirmed(ConversationDto conversation)
    {
        return conversation.Confirm?.UserAt != null && conversation.Confirm.ReaderAt != null;
    }
}
