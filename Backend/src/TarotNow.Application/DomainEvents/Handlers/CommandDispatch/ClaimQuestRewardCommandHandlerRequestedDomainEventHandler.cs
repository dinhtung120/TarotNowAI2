using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Gamification.Commands;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ClaimQuestRewardCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ClaimQuestRewardCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ClaimQuestRewardCommand, ClaimQuestRewardResult> _executor;

    public ClaimQuestRewardCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ClaimQuestRewardCommand, ClaimQuestRewardResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ClaimQuestRewardCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}
