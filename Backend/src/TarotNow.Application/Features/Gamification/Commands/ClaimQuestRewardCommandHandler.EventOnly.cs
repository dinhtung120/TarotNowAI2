using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class ClaimQuestRewardCommandHandler : IRequestHandler<ClaimQuestRewardCommand, ClaimQuestRewardResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public ClaimQuestRewardCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ClaimQuestRewardResult> Handle(ClaimQuestRewardCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ClaimQuestRewardCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ClaimQuestRewardResult)domainEvent.Result;
    }
}

public sealed class ClaimQuestRewardCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public ClaimQuestRewardCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public ClaimQuestRewardCommandHandlerRequestedDomainEvent(ClaimQuestRewardCommand command)
    {
        Command = command;
    }
}
