using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

// Command để reader chấp nhận conversation đang chờ.
public class AcceptConversationCommand : IRequest<ConversationActionResult>
{
    // Định danh conversation cần accept.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh reader thực hiện thao tác accept.
    public Guid ReaderId { get; set; }
}
