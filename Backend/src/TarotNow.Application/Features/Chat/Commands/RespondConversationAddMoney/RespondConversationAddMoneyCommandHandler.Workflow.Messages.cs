using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandler
{
    private async Task<ChatMessageDto> SendAcceptMessageAsync(
        RespondConversationAddMoneyCommand request,
        ChatMessageDto offer,
        CancellationToken cancellationToken)
    {
        var content = BuildOfferResponseContent(offer.Id, offer.PaymentPayload?.ProposalId, note: null);
        return await _mediator.Send(new SendMessageCommand
        {
            ConversationId = request.ConversationId,
            SenderId = request.UserId,
            Type = ChatMessageType.PaymentAccept,
            Content = content
        }, cancellationToken);
    }

    private async Task<ChatMessageDto> GetOfferMessageAsync(
        RespondConversationAddMoneyCommand request,
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.OfferMessageId))
        {
            throw new BadRequestException("OfferMessageId là bắt buộc.");
        }

        var offer = await _chatMessageRepository.GetByIdAsync(request.OfferMessageId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đề xuất cộng tiền.");

        if (offer.ConversationId != conversation.Id)
        {
            throw new BadRequestException("Đề xuất cộng tiền không thuộc cuộc trò chuyện này.");
        }

        if (offer.Type != ChatMessageType.PaymentOffer)
        {
            throw new BadRequestException("Tin nhắn được chọn không phải đề xuất cộng tiền.");
        }

        if (offer.PaymentPayload == null || offer.PaymentPayload.AmountDiamond <= 0)
        {
            throw new BadRequestException("Đề xuất cộng tiền không hợp lệ.");
        }

        return offer;
    }

    private async Task EnsureOfferNotHandledAsync(
        string conversationId,
        string offerMessageId,
        CancellationToken cancellationToken)
    {
        var handled = await _chatMessageRepository.HasPaymentOfferResponseAsync(
            conversationId,
            offerMessageId,
            cancellationToken);

        if (handled)
        {
            throw new BadRequestException("Đề xuất cộng tiền này đã được xử lý.");
        }
    }

    private static string BuildOfferIdempotencyKey(ChatMessageDto offer)
    {
        if (string.IsNullOrWhiteSpace(offer.PaymentPayload?.ProposalId) == false)
        {
            return $"offer_{offer.PaymentPayload!.ProposalId}";
        }

        return $"offer_msg_{offer.Id}";
    }

    private static string BuildOfferResponseContent(
        string offerMessageId,
        string? proposalId,
        string? note)
    {
        var escapedNote = note?.Replace("\"", "\\\"");
        var noteJson = string.IsNullOrWhiteSpace(escapedNote)
            ? string.Empty
            : $",\"note\":\"{escapedNote}\"";

        var safeProposal = proposalId?.Replace("\"", "\\\"") ?? string.Empty;
        return $"{{\"offerMessageId\":\"{offerMessageId}\",\"proposalId\":\"{safeProposal}\"{noteJson}}}";
    }
}
