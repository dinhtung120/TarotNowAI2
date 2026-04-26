using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class AddQuestionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AddQuestionCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<AddQuestionCommand, Guid> _executor;

    public AddQuestionCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<AddQuestionCommand, Guid> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        AddQuestionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}
