using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public LogoutCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new LogoutCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result;
    }
}

public sealed class LogoutCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public LogoutCommand Command { get; }

    public bool Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public LogoutCommandHandlerRequestedDomainEvent(LogoutCommand command)
    {
        Command = command;
    }
}
