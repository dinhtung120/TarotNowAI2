using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

public sealed class DailyCheckInCommandHandler : IRequestHandler<DailyCheckInCommand, DailyCheckInResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public DailyCheckInCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<DailyCheckInResult> Handle(DailyCheckInCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new DailyCheckInCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (DailyCheckInResult)domainEvent.Result;
    }
}

public sealed class DailyCheckInCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public DailyCheckInCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public DailyCheckInCommandHandlerRequestedDomainEvent(DailyCheckInCommand command)
    {
        Command = command;
    }
}
