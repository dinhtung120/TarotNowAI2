using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class AdminQuestCommandHandler : IRequestHandler<UpsertQuestDefinitionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public AdminQuestCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(UpsertQuestDefinitionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new AdminQuestCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class AdminQuestCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public UpsertQuestDefinitionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public AdminQuestCommandHandlerRequestedDomainEvent(UpsertQuestDefinitionCommand command)
    {
        Command = command;
    }
}
