using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

// Command để reader từ chối conversation đang chờ.
public class RejectConversationCommand : IRequest<ConversationActionResult>
{
    // Định danh conversation cần reject.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh reader thực hiện reject.
    public Guid ReaderId { get; set; }

    // Lý do từ chối conversation (tùy chọn).
    public string? Reason { get; set; }
}
