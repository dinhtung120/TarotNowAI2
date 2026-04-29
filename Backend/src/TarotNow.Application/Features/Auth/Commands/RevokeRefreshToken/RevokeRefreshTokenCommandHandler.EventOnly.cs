using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RevokeRefreshTokenCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RevokeRefreshTokenCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result;
    }
}

public sealed class RevokeRefreshTokenCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RevokeRefreshTokenCommand Command { get; }

    public bool Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RevokeRefreshTokenCommandHandlerRequestedDomainEvent(RevokeRefreshTokenCommand command)
    {
        Command = command;
    }
}
