using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandler
{
    private async Task<ConversationDto> GetConversationAsync(
        RespondConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        return await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");
    }

    private static void ValidateRespondPermission(ConversationDto conversation, Guid userId)
    {
        if (conversation.UserId != userId.ToString())
        {
            throw new BadRequestException("Bạn không thể phản hồi cộng tiền cho cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            throw new BadRequestException($"Không thể phản hồi cộng tiền ở trạng thái '{conversation.Status}'.");
        }
    }

    private static Guid ValidateAndParseReaderId(ConversationDto conversation, RespondConversationAddMoneyCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.OfferMessageId))
        {
            throw new BadRequestException("OfferMessageId là bắt buộc.");
        }
        if (Guid.TryParse(conversation.ReaderId, out var readerId) == false)
        {
            throw new BadRequestException("ReaderId không hợp lệ.");
        }

        return readerId;
    }

    private async Task<ConversationAddMoneyRespondResult> RejectOfferAsync(
        RespondConversationAddMoneyCommand request,
        ChatMessageDto offer,
        CancellationToken cancellationToken)
    {
        var rejectContent = BuildOfferResponseContent(
            offer.Id,
            offer.PaymentPayload?.ProposalId,
            request.RejectReason);

        var message = await _mediator.Send(new SendMessageCommand
        {
            ConversationId = request.ConversationId,
            SenderId = request.UserId,
            Type = ChatMessageType.PaymentReject,
            Content = rejectContent
        }, cancellationToken);

        return new ConversationAddMoneyRespondResult
        {
            Accepted = false,
            MessageId = message.Id
        };
    }

    private async Task<Guid> FreezeOfferAsync(
        RespondConversationAddMoneyCommand request,
        ChatMessageDto offer,
        Guid readerId,
        CancellationToken cancellationToken)
    {
        var amountDiamond = offer.PaymentPayload?.AmountDiamond ?? 0;
        if (amountDiamond <= 0)
        {
            throw new BadRequestException("Đề xuất cộng tiền không hợp lệ.");
        }

        if (offer.PaymentPayload?.ExpiresAt is DateTime expiresAt && expiresAt <= DateTime.UtcNow)
        {
            throw new BadRequestException("Đề xuất cộng tiền đã hết hạn.");
        }

        var idempotencyKey = BuildOfferIdempotencyKey(offer);
        return await _mediator.Send(new AddQuestionCommand
        {
            UserId = request.UserId,
            ConversationRef = request.ConversationId,
            AmountDiamond = amountDiamond,
            ProposalMessageRef = offer.Id,
            IdempotencyKey = idempotencyKey
        }, cancellationToken);
    }

}
