using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

public sealed class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public CreateConversationCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new CreateConversationCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationDto)domainEvent.Result;
    }
}

public sealed class CreateConversationCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public CreateConversationCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public CreateConversationCommandHandlerRequestedDomainEvent(CreateConversationCommand command)
    {
        Command = command;
    }
}
