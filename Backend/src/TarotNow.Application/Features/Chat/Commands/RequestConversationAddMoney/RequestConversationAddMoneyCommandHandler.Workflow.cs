using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public partial class RequestConversationAddMoneyCommandHandler
{
    /// <summary>
    /// Tải conversation và kiểm tra điều kiện hợp lệ cho đề nghị cộng tiền.
    /// Luồng xử lý: kiểm tra conversation tồn tại, reader ownership và trạng thái ongoing.
    /// </summary>
    private async Task<ConversationDto> LoadConversationAsync(
        RequestConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.ReaderId != request.ReaderId.ToString())
        {
            // Chỉ reader của conversation mới được gửi yêu cầu cộng tiền.
            throw new BadRequestException("Bạn không thể tạo yêu cầu cộng tiền cho cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            // Chỉ cho phép cộng tiền khi conversation đang ongoing.
            throw new BadRequestException($"Không thể cộng tiền ở trạng thái '{conversation.Status}'.");
        }

        return conversation;
    }

    /// <summary>
    /// Xử lý nhánh đặc biệt khi conversation đang có yêu cầu complete.
    /// Luồng xử lý: nếu có complete request thì gửi system message hủy đề nghị cộng tiền và trả kết quả ngay.
    /// </summary>
    private async Task<ConversationAddMoneyRequestResult?> TrySendCancelledByCompleteMessageAsync(
        RequestConversationAddMoneyCommand request,
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(conversation.Confirm?.RequestedBy))
        {
            return null;
        }

        // Gửi system message thay cho payment offer để tránh xung đột flow complete conversation.
        var system = await _mediator.Send(new SendMessageCommand
        {
            ConversationId = request.ConversationId,
            SenderId = request.ReaderId,
            Type = ChatMessageType.System,
            Content = "Yêu cầu cộng thêm tiền đã bị hủy do một bên yêu cầu hoàn thành cuộc trò chuyện."
        }, cancellationToken);

        return new ConversationAddMoneyRequestResult { MessageId = system.Id };
    }

    /// <summary>
    /// Đảm bảo conversation chưa có payment offer pending.
    /// Luồng xử lý: tìm offer pending gần nhất và ném lỗi nếu tồn tại.
    /// </summary>
    private async Task EnsureNoPendingOfferAsync(
        string conversationId,
        CancellationToken cancellationToken)
    {
        var pendingOffer = await _chatMessageRepository.FindLatestPendingPaymentOfferAsync(
            conversationId,
            cancellationToken);

        if (pendingOffer != null)
        {
            // Rule nghiệp vụ: mỗi thời điểm chỉ cho phép một offer pending.
            throw new BadRequestException("Đã có một yêu cầu cộng tiền đang chờ phản hồi.");
        }
    }

    /// <summary>
    /// Gửi payment offer message vào conversation.
    /// Luồng xử lý: gọi SendMessageCommand với payload payment và hạn phản hồi 24 giờ.
    /// </summary>
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
                // ProposalId dùng idempotency key để nhận diện duy nhất offer.
                AmountDiamond = request.AmountDiamond,
                Description = request.Description,
                ProposalId = request.IdempotencyKey,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            }
        }, cancellationToken);
    }
}
