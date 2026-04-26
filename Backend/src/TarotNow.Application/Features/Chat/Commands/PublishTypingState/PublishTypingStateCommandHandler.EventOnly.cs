using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Chat.Commands.PublishTypingState;

public sealed class PublishTypingStateCommandHandler : IRequestHandler<PublishTypingStateCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public PublishTypingStateCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(PublishTypingStateCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new PublishTypingStateCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class PublishTypingStateCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public PublishTypingStateCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public PublishTypingStateCommandHandlerRequestedDomainEvent(PublishTypingStateCommand command)
    {
        Command = command;
    }
}
