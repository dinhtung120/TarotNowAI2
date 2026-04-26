using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public sealed class RequestConversationCompleteCommandHandler : IRequestHandler<RequestConversationCompleteCommand, ConversationActionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RequestConversationCompleteCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationActionResult> Handle(RequestConversationCompleteCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RequestConversationCompleteCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationActionResult)domainEvent.Result;
    }
}

public sealed class RequestConversationCompleteCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RequestConversationCompleteCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RequestConversationCompleteCommandHandlerRequestedDomainEvent(RequestConversationCompleteCommand command)
    {
        Command = command;
    }
}
