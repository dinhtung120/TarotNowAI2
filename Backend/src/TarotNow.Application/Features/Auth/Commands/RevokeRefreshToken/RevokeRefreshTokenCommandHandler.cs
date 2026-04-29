using MediatR;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.RevokeToken;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Auth.Commands.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RevokeRefreshTokenCommandHandlerRequestedDomainEvent>
{
    private readonly IMediator _mediator;

    public RevokeRefreshTokenCommandHandlerRequestedDomainEventHandler(
        IMediator mediator,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _mediator = mediator;
    }

    public Task<bool> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var command = new RevokeTokenCommand
        {
            Token = request.Token,
            RevokeAll = request.RevokeAll,
            UserId = request.UserId
        };
        return _mediator.Send(command, cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        RevokeRefreshTokenCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
