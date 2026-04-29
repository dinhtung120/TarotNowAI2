using MediatR;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.RevokeRefreshToken;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<LogoutCommandHandlerRequestedDomainEvent>
{
    private readonly IMediator _mediator;

    public LogoutCommandHandlerRequestedDomainEventHandler(
        IMediator mediator,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _mediator = mediator;
    }

    public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var command = new RevokeRefreshTokenCommand
        {
            Token = request.Token,
            RevokeAll = request.RevokeAll,
            UserId = request.UserId
        };
        return _mediator.Send(command, cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        LogoutCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
