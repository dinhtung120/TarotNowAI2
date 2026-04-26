using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public sealed class RejectConversationCommandHandler : IRequestHandler<RejectConversationCommand, ConversationActionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RejectConversationCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationActionResult> Handle(RejectConversationCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RejectConversationCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationActionResult)domainEvent.Result;
    }
}

public sealed class RejectConversationCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RejectConversationCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RejectConversationCommandHandlerRequestedDomainEvent(RejectConversationCommand command)
    {
        Command = command;
    }
}
