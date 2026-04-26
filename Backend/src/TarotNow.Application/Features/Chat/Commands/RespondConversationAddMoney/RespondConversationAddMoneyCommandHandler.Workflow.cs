using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Tải conversation theo id từ request phản hồi cộng tiền.
    /// Luồng xử lý: lấy conversation từ repository và ném lỗi nếu không tồn tại.
    /// </summary>
    private async Task<ConversationDto> GetConversationAsync(
        RespondConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        return await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");
    }

    /// <summary>
    /// Kiểm tra quyền phản hồi đề nghị cộng tiền của user hiện tại.
    /// Luồng xử lý: xác nhận user thuộc conversation và conversation đang ở trạng thái Ongoing.
    /// </summary>
    private static void ValidateRespondPermission(ConversationDto conversation, Guid userId)
    {
        if (conversation.UserId != userId.ToString())
        {
            // Chặn user ngoài conversation can thiệp vào offer cộng tiền.
            throw new BadRequestException("Bạn không thể phản hồi cộng tiền cho cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            // Chỉ cho phép phản hồi offer khi conversation đang hoạt động.
            throw new BadRequestException($"Không thể phản hồi cộng tiền ở trạng thái '{conversation.Status}'.");
        }
    }

    /// <summary>
    /// Xử lý nhánh từ chối đề nghị cộng tiền.
    /// Luồng xử lý: dựng payload reject từ offer gốc, gửi payment reject message, rồi trả kết quả rejected.
    /// </summary>
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

    /// <summary>
    /// Đóng băng thêm tiền theo nội dung đề nghị cộng tiền đã được user chấp nhận.
    /// Luồng xử lý: kiểm tra amount/expiresAt hợp lệ, dựng idempotency key từ offer, rồi gửi AddQuestionCommand để freeze.
    /// </summary>
    private async Task<Guid> FreezeOfferAsync(
        RespondConversationAddMoneyCommand request,
        ChatMessageDto offer,
        CancellationToken cancellationToken)
    {
        var amountDiamond = offer.PaymentPayload?.AmountDiamond ?? 0;
        if (amountDiamond <= 0)
        {
            // Business rule: offer không có amount dương thì không được freeze.
            throw new BadRequestException("Đề xuất cộng tiền không hợp lệ.");
        }

        if (offer.PaymentPayload?.ExpiresAt is DateTime expiresAt && expiresAt <= DateTime.UtcNow)
        {
            // Chặn xử lý offer đã hết hạn để tránh freeze sau deadline thỏa thuận.
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
