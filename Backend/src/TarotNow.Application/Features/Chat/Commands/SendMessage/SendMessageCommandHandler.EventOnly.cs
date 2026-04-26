using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public sealed class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ChatMessageDto>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public SendMessageCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new SendMessageCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ChatMessageDto)domainEvent.Result;
    }
}

public sealed class SendMessageCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public SendMessageCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public SendMessageCommandHandlerRequestedDomainEvent(SendMessageCommand command)
    {
        Command = command;
    }
}
