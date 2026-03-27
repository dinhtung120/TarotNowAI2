using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public class RequestConversationCompleteCommand : IRequest<ConversationActionResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid RequesterId { get; set; }
}
