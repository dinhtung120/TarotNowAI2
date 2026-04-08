using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandler
{
    /// <summary>
    /// Gửi message xác nhận chấp nhận đề nghị cộng tiền.
    /// Luồng xử lý: dựng payload accept từ offer gốc và gửi SendMessageCommand kiểu PaymentAccept.
    /// </summary>
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

    /// <summary>
    /// Lấy offer message cần phản hồi và kiểm tra tính hợp lệ.
    /// Luồng xử lý: validate offer id bắt buộc, tải message, kiểm tra conversation/type/payload để đảm bảo phản hồi đúng offer.
    /// </summary>
    private async Task<ChatMessageDto> GetOfferMessageAsync(
        RespondConversationAddMoneyCommand request,
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.OfferMessageId))
        {
            // Không có offer id thì không thể xác định đề nghị cần phản hồi.
            throw new BadRequestException("OfferMessageId là bắt buộc.");
        }

        var offer = await _chatMessageRepository.GetByIdAsync(request.OfferMessageId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đề xuất cộng tiền.");

        if (offer.ConversationId != conversation.Id)
        {
            // Chặn việc dùng offer của conversation khác để tránh thao tác sai ngữ cảnh.
            throw new BadRequestException("Đề xuất cộng tiền không thuộc cuộc trò chuyện này.");
        }

        if (offer.Type != ChatMessageType.PaymentOffer)
        {
            // Chỉ payment offer mới được phép đi qua flow phản hồi này.
            throw new BadRequestException("Tin nhắn được chọn không phải đề xuất cộng tiền.");
        }

        if (offer.PaymentPayload == null || offer.PaymentPayload.AmountDiamond <= 0)
        {
            // Payload thiếu hoặc amount không hợp lệ thì phải dừng để bảo toàn nghiệp vụ tiền.
            throw new BadRequestException("Đề xuất cộng tiền không hợp lệ.");
        }

        return offer;
    }

    /// <summary>
    /// Đảm bảo offer chưa từng được phản hồi trước đó.
    /// Luồng xử lý: kiểm tra cờ handled theo conversation + offer id và ném lỗi khi đã có phản hồi.
    /// </summary>
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

    /// <summary>
    /// Dựng idempotency key cho thao tác freeze theo offer.
    /// Luồng xử lý: ưu tiên ProposalId từ payload; nếu thiếu thì fallback theo message id để vẫn đảm bảo duy nhất.
    /// </summary>
    private static string BuildOfferIdempotencyKey(ChatMessageDto offer)
    {
        if (string.IsNullOrWhiteSpace(offer.PaymentPayload?.ProposalId) == false)
        {
            return $"offer_{offer.PaymentPayload!.ProposalId}";
        }

        return $"offer_msg_{offer.Id}";
    }

    /// <summary>
    /// Dựng JSON payload phản hồi offer để lưu trong chat message.
    /// Luồng xử lý: escape ký tự nháy cho note/proposalId rồi ghép JSON nhằm giữ payload hợp lệ khi parse downstream.
    /// </summary>
    private static string BuildOfferResponseContent(
        string offerMessageId,
        string? proposalId,
        string? note)
    {
        // Escape chuỗi trước khi nội suy JSON thủ công để tránh lỗi cú pháp payload.
        var escapedNote = note?.Replace("\"", "\\\"");
        var noteJson = string.IsNullOrWhiteSpace(escapedNote)
            ? string.Empty
            : $",\"note\":\"{escapedNote}\"";

        var safeProposal = proposalId?.Replace("\"", "\\\"") ?? string.Empty;
        return $"{{\"offerMessageId\":\"{offerMessageId}\",\"proposalId\":\"{safeProposal}\"{noteJson}}}";
    }
}
