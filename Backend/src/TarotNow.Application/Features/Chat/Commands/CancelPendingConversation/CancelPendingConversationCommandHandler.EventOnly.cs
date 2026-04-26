using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;

public sealed class CancelPendingConversationCommandHandler : IRequestHandler<CancelPendingConversationCommand, ConversationActionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public CancelPendingConversationCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationActionResult> Handle(CancelPendingConversationCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new CancelPendingConversationCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationActionResult)domainEvent.Result;
    }
}

public sealed class CancelPendingConversationCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public CancelPendingConversationCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public CancelPendingConversationCommandHandlerRequestedDomainEvent(CancelPendingConversationCommand command)
    {
        Command = command;
    }
}
