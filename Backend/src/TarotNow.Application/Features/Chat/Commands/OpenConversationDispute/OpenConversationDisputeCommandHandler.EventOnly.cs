using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;

public sealed class OpenConversationDisputeCommandHandler : IRequestHandler<OpenConversationDisputeCommand, ConversationActionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public OpenConversationDisputeCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationActionResult> Handle(OpenConversationDisputeCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new OpenConversationDisputeCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationActionResult)domainEvent.Result;
    }
}

public sealed class OpenConversationDisputeCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public OpenConversationDisputeCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public OpenConversationDisputeCommandHandlerRequestedDomainEvent(OpenConversationDisputeCommand command)
    {
        Command = command;
    }
}
