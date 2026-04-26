using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class AdminTitleCommandHandler : IRequestHandler<UpsertTitleDefinitionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public AdminTitleCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(UpsertTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new AdminTitleCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class AdminTitleCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public UpsertTitleDefinitionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public AdminTitleCommandHandlerRequestedDomainEvent(UpsertTitleDefinitionCommand command)
    {
        Command = command;
    }
}
