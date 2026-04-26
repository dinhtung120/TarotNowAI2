using MediatR;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Presence.Commands.PublishUserStatusChanged;

/// <summary>
/// Command publish trạng thái hiện diện mới của user.
/// </summary>
public sealed class PublishUserStatusChangedCommand : IRequest<bool>
{
    /// <summary>
    /// Định danh user.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái hiện diện (online/offline).
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Event-only handler phát requested domain event cho luồng presence.
/// </summary>
public sealed class PublishUserStatusChangedCommandHandler
    : IRequestHandler<PublishUserStatusChangedCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public PublishUserStatusChangedCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(PublishUserStatusChangedCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new PublishUserStatusChangedCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class PublishUserStatusChangedCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public PublishUserStatusChangedCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public PublishUserStatusChangedCommandHandlerRequestedDomainEvent(PublishUserStatusChangedCommand command)
    {
        Command = command;
    }
}

/// <summary>
/// Requested domain event handler cho publish trạng thái online/offline.
/// </summary>
public sealed class PublishUserStatusChangedCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PublishUserStatusChangedCommandHandlerRequestedDomainEvent>
{
    private static readonly HashSet<string> AllowedStatuses =
        new(StringComparer.OrdinalIgnoreCase) { "online", "offline" };

    private readonly IDomainEventPublisher _domainEventPublisher;

    public PublishUserStatusChangedCommandHandlerRequestedDomainEventHandler(
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    protected override async Task HandleDomainEventAsync(
        PublishUserStatusChangedCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var request = domainEvent.Command;
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            throw new BadRequestException("UserId is required.");
        }

        var normalizedStatus = request.Status.Trim().ToLowerInvariant();
        if (AllowedStatuses.Contains(normalizedStatus) == false)
        {
            throw new BadRequestException("Status must be online or offline.");
        }

        await _domainEventPublisher.PublishAsync(
            new UserStatusChangedDomainEvent
            {
                UserId = request.UserId.Trim(),
                Status = normalizedStatus,
                OccurredAtUtc = DateTime.UtcNow
            },
            cancellationToken);

        domainEvent.Result = true;
    }
}
