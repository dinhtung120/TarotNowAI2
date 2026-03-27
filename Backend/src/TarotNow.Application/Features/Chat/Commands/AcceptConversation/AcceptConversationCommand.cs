using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public class AcceptConversationCommand : IRequest<ConversationActionResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid ReaderId { get; set; }
}
