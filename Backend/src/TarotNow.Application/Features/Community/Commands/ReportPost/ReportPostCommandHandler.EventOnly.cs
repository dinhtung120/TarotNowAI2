using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Community.Commands.ReportPost;

public sealed class ReportPostCommandHandler : IRequestHandler<ReportPostCommand, ReportDto>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public ReportPostCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ReportDto> Handle(ReportPostCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ReportPostCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ReportDto)domainEvent.Result;
    }
}

public sealed class ReportPostCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public ReportPostCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public ReportPostCommandHandlerRequestedDomainEvent(ReportPostCommand command)
    {
        Command = command;
    }
}
