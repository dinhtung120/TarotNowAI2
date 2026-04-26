using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class DeleteQuestDefinitionCommandHandler : IRequestHandler<DeleteQuestDefinitionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public DeleteQuestDefinitionCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(DeleteQuestDefinitionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new DeleteQuestDefinitionCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class DeleteQuestDefinitionCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public DeleteQuestDefinitionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public DeleteQuestDefinitionCommandHandlerRequestedDomainEvent(DeleteQuestDefinitionCommand command)
    {
        Command = command;
    }
}

public sealed class DeleteAchievementDefinitionCommandHandler : IRequestHandler<DeleteAchievementDefinitionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public DeleteAchievementDefinitionCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(DeleteAchievementDefinitionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new DeleteAchievementDefinitionCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class DeleteAchievementDefinitionCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public DeleteAchievementDefinitionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public DeleteAchievementDefinitionCommandHandlerRequestedDomainEvent(DeleteAchievementDefinitionCommand command)
    {
        Command = command;
    }
}

public sealed class DeleteTitleDefinitionCommandHandler : IRequestHandler<DeleteTitleDefinitionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public DeleteTitleDefinitionCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(DeleteTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new DeleteTitleDefinitionCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class DeleteTitleDefinitionCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public DeleteTitleDefinitionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public DeleteTitleDefinitionCommandHandlerRequestedDomainEvent(DeleteTitleDefinitionCommand command)
    {
        Command = command;
    }
}
