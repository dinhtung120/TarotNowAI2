using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public UpdateUserCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new UpdateUserCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class UpdateUserCommandHandlerRequestedDomainEvent : IIdempotentDomainEvent
{
    public UpdateUserCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public string EventIdempotencyKey => CommandEventIdempotencyKey.Build(
        nameof(UpdateUserCommandHandlerRequestedDomainEvent),
        Command.IdempotencyKey);

    public UpdateUserCommandHandlerRequestedDomainEvent(UpdateUserCommand command)
    {
        Command = command;
    }
}
