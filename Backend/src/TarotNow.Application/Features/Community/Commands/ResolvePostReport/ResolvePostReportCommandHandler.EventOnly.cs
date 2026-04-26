using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Community.Commands.ResolvePostReport;

public sealed class ResolvePostReportCommandHandler : IRequestHandler<ResolvePostReportCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public ResolvePostReportCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(ResolvePostReportCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ResolvePostReportCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class ResolvePostReportCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public ResolvePostReportCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public ResolvePostReportCommandHandlerRequestedDomainEvent(ResolvePostReportCommand command)
    {
        Command = command;
    }
}
