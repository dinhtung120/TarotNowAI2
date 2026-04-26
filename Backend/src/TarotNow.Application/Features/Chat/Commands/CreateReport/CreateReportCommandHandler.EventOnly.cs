using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

public sealed class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportDto>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public CreateReportCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new CreateReportCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ReportDto)domainEvent.Result;
    }
}

public sealed class CreateReportCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public CreateReportCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public CreateReportCommandHandlerRequestedDomainEvent(CreateReportCommand command)
    {
        Command = command;
    }
}
