using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public class RespondConversationCompleteCommand : IRequest<ConversationCompleteRespondResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid RequesterId { get; set; }

    public bool Accept { get; set; }
}
