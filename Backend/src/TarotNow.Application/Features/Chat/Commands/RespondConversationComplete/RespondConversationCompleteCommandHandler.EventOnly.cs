using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public sealed class RespondConversationCompleteCommandHandler : IRequestHandler<RespondConversationCompleteCommand, ConversationCompleteRespondResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RespondConversationCompleteCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationCompleteRespondResult> Handle(RespondConversationCompleteCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RespondConversationCompleteCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationCompleteRespondResult)domainEvent.Result;
    }
}

public sealed class RespondConversationCompleteCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RespondConversationCompleteCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RespondConversationCompleteCommandHandlerRequestedDomainEvent(RespondConversationCompleteCommand command)
    {
        Command = command;
    }
}
