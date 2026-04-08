using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

// Command yêu cầu hoàn thành conversation từ một participant.
public class RequestConversationCompleteCommand : IRequest<ConversationActionResult>
{
    // Định danh conversation cần yêu cầu hoàn thành.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh participant gửi yêu cầu.
    public Guid RequesterId { get; set; }
}
