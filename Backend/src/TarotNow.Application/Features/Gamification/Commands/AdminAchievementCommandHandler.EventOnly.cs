using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class AdminAchievementCommandHandler : IRequestHandler<UpsertAchievementDefinitionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public AdminAchievementCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(UpsertAchievementDefinitionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new AdminAchievementCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class AdminAchievementCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public UpsertAchievementDefinitionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public AdminAchievementCommandHandlerRequestedDomainEvent(UpsertAchievementDefinitionCommand command)
    {
        Command = command;
    }
}
