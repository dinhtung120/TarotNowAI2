using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
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
/// Handler phát UserStatusChangedDomainEvent cho luồng realtime presence.
/// </summary>
public sealed class PublishUserStatusChangedCommandHandler
    : IRequestHandler<PublishUserStatusChangedCommand, bool>
{
    private static readonly HashSet<string> AllowedStatuses =
        new(StringComparer.OrdinalIgnoreCase) { "online", "offline" };

    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler publish user status changed.
    /// </summary>
    public PublishUserStatusChangedCommandHandler(IDomainEventPublisher domainEventPublisher)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    public async Task<bool> Handle(PublishUserStatusChangedCommand request, CancellationToken cancellationToken)
    {
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

        return true;
    }
}
