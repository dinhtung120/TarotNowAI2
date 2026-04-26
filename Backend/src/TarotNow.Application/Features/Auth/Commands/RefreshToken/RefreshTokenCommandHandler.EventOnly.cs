using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RefreshTokenCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RefreshTokenCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (RefreshTokenResult)domainEvent.Result;
    }
}

public sealed class RefreshTokenCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RefreshTokenCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RefreshTokenCommandHandlerRequestedDomainEvent(RefreshTokenCommand command)
    {
        Command = command;
    }
}
