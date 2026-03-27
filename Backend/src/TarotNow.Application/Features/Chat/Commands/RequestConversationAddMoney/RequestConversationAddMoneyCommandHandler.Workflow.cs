using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public partial class RequestConversationAddMoneyCommandHandler
{
    private async Task<ConversationDto> LoadConversationAsync(
        RequestConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.ReaderId != request.ReaderId.ToString())
        {
            throw new BadRequestException("Bạn không thể tạo yêu cầu cộng tiền cho cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            throw new BadRequestException($"Không thể cộng tiền ở trạng thái '{conversation.Status}'.");
        }

        return conversation;
    }

    private async Task<ConversationAddMoneyRequestResult?> TrySendCancelledByCompleteMessageAsync(
        RequestConversationAddMoneyCommand request,
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(conversation.Confirm?.RequestedBy))
        {
            return null;
        }

        var system = await _mediator.Send(new SendMessageCommand
        {
            ConversationId = request.ConversationId,
            SenderId = request.ReaderId,
            Type = ChatMessageType.System,
            Content = "Yêu cầu cộng thêm tiền đã bị hủy do một bên yêu cầu hoàn thành cuộc trò chuyện."
        }, cancellationToken);

        return new ConversationAddMoneyRequestResult { MessageId = system.Id };
    }

    private async Task EnsureNoPendingOfferAsync(
        string conversationId,
        CancellationToken cancellationToken)
    {
        var pendingOffer = await _chatMessageRepository.FindLatestPendingPaymentOfferAsync(
            conversationId,
            cancellationToken);

        if (pendingOffer != null)
        {
            throw new BadRequestException("Đã có một yêu cầu cộng tiền đang chờ phản hồi.");
        }
    }

    private Task<ChatMessageDto> SendOfferMessageAsync(
        RequestConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        return _mediator.Send(new SendMessageCommand
        {
            ConversationId = request.ConversationId,
            SenderId = request.ReaderId,
            Type = ChatMessageType.PaymentOffer,
            Content = request.Description!.Trim(),
            PaymentPayload = new PaymentPayloadDto
            {
                AmountDiamond = request.AmountDiamond,
                Description = request.Description,
                ProposalId = request.IdempotencyKey,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            }
        }, cancellationToken);
    }
}
