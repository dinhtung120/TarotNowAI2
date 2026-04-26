using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Admin.Commands.AddUserBalance;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class AddUserBalanceCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AddUserBalanceCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<AddUserBalanceCommand, bool> _executor;

    public AddUserBalanceCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<AddUserBalanceCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        AddUserBalanceCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}
