using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public class RejectConversationCommand : IRequest<ConversationActionResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid ReaderId { get; set; }

    public string? Reason { get; set; }
}
