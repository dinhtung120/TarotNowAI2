using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

public sealed class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public AddUserBalanceCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new AddUserBalanceCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class AddUserBalanceCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public AddUserBalanceCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public AddUserBalanceCommandHandlerRequestedDomainEvent(AddUserBalanceCommand command)
    {
        Command = command;
    }
}
