using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public class RequestConversationAddMoneyCommand : IRequest<ConversationAddMoneyRequestResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid ReaderId { get; set; }

    public long AmountDiamond { get; set; }

    public string? Description { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}
