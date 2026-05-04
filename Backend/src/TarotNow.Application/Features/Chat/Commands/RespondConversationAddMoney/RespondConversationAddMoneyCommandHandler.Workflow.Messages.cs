using System.Text.Json;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

using RespondAddMoneyErrorCodes = TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney.RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler.ErrorCodes;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler
{
    private const string DefaultRejectReason = "User từ chối đề xuất cộng tiền.";

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
            throw new BusinessRuleException(
                RespondAddMoneyErrorCodes.InvalidOffer,
                "Đề xuất cộng tiền không thuộc cuộc trò chuyện này.");
        }

        if (offer.Type != ChatMessageType.PaymentOffer)
        {
            // Chỉ payment offer mới được phép đi qua flow phản hồi này.
            throw new BusinessRuleException(
                RespondAddMoneyErrorCodes.InvalidOffer,
                "Tin nhắn được chọn không phải đề xuất cộng tiền.");
        }

        if (offer.PaymentPayload == null || offer.PaymentPayload.AmountDiamond <= 0)
        {
            // Payload thiếu hoặc amount không hợp lệ thì phải dừng để bảo toàn nghiệp vụ tiền.
            throw new BusinessRuleException(
                RespondAddMoneyErrorCodes.InvalidOffer,
                "Đề xuất cộng tiền không hợp lệ.");
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
            throw new BusinessRuleException(
                RespondAddMoneyErrorCodes.AlreadyHandled,
                "Đề xuất cộng tiền này đã được xử lý.");
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
    internal static string BuildOfferResponseContent(
        string offerMessageId,
        string? proposalId,
        string? note)
    {
        var payload = new Dictionary<string, string?>
        {
            ["offerMessageId"] = offerMessageId,
            ["proposalId"] = proposalId ?? string.Empty
        };
        if (!string.IsNullOrWhiteSpace(note))
        {
            payload["note"] = note;
        }

        return JsonSerializer.Serialize(payload);
    }

    internal static string NormalizeRejectReason(string? rawReason)
    {
        var normalized = rawReason?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? DefaultRejectReason : normalized;
    }
}
