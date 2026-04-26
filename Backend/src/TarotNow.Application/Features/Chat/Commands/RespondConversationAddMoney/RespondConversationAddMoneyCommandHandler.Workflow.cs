using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandler
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
    /// Kiểm tra OfferMessageId bắt buộc và parse ReaderId từ conversation.
    /// Luồng xử lý: validate dữ liệu bắt buộc, parse reader id để dùng cho lệnh freeze.
    /// </summary>
    private static Guid ValidateAndParseReaderId(ConversationDto conversation, RespondConversationAddMoneyCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.OfferMessageId))
        {
            // OfferMessageId là khóa chính để ràng buộc phản hồi vào đúng đề nghị.
            throw new BadRequestException("OfferMessageId là bắt buộc.");
        }
        if (Guid.TryParse(conversation.ReaderId, out var readerId) == false)
        {
            // Edge case dữ liệu conversation lỗi định dạng reader id.
            throw new BadRequestException("ReaderId không hợp lệ.");
        }

        return readerId;
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
        Guid readerId,
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

    /// <summary>
    /// Compensation khi đã freeze tiền nhưng gửi accept message thất bại.
    /// </summary>
    private async Task CompensateOfferFreezeAsync(
        RespondConversationAddMoneyCommand request,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepository.GetItemForUpdateAsync(itemId, transactionCt);
            if (item == null || item.Status == QuestionItemStatus.Refunded)
            {
                return;
            }

            var refundAmount = item.AmountDiamond;
            await _walletRepository.RefundAsync(
                request.UserId,
                refundAmount,
                referenceSource: "offer_accept_compensation",
                referenceId: item.Id.ToString(),
                description: $"Compensate add-money freeze {refundAmount}💎",
                idempotencyKey: $"compensate_offer_accept_{item.Id}",
                cancellationToken: transactionCt);

            item.Status = QuestionItemStatus.Refunded;
            item.RefundedAt = DateTime.UtcNow;
            item.UpdatedAt = item.RefundedAt;
            await _financeRepository.UpdateItemAsync(item, transactionCt);

            var session = await _financeRepository.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.TotalFrozen = Math.Max(0, session.TotalFrozen - refundAmount);
                if (session.TotalFrozen == 0 && session.Status == ChatFinanceSessionStatus.Active)
                {
                    session.Status = ChatFinanceSessionStatus.Refunded;
                }

                session.UpdatedAt = DateTime.UtcNow;
                await _financeRepository.UpdateSessionAsync(session, transactionCt);
            }

            await _financeRepository.SaveChangesAsync(transactionCt);
            await _domainEventPublisher.PublishAsync(
                new Domain.Events.MoneyChangedDomainEvent
                {
                    UserId = request.UserId,
                    Currency = CurrencyType.Diamond,
                    ChangeType = TransactionType.EscrowRefund,
                    DeltaAmount = refundAmount,
                    ReferenceId = item.Id.ToString()
                },
                transactionCt);
        }, cancellationToken);
    }

}
