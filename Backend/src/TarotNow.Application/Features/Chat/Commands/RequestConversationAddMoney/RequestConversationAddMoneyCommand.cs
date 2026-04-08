using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

// Command để reader gửi đề nghị cộng thêm tiền trong conversation.
public class RequestConversationAddMoneyCommand : IRequest<ConversationAddMoneyRequestResult>
{
    // Định danh conversation mục tiêu.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh reader gửi đề nghị.
    public Guid ReaderId { get; set; }

    // Số kim cương đề nghị cộng thêm.
    public long AmountDiamond { get; set; }

    // Mô tả lý do đề nghị cộng tiền.
    public string? Description { get; set; }

    // Khóa idempotency cho proposal hiện tại.
    public string IdempotencyKey { get; set; } = string.Empty;
}
