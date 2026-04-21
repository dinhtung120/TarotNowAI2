using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReaderRequestReviewRequestedDomainEventHandler
{
    private async Task RevokeSessionsAfterRoleChangeAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeSessionIds = await _authSessionRepository.GetActiveSessionIdsByUserAsync(userId, cancellationToken);
        await _refreshTokenRepository.RevokeAllByUserIdAsync(userId, cancellationToken);
        await _authSessionRepository.RevokeAllByUserAsync(userId, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new UserLoggedOutDomainEvent
            {
                UserId = userId,
                RevokeAll = true,
                SessionIds = activeSessionIds,
                Reason = RefreshRevocationReasons.RoleChanged
            },
            cancellationToken);
    }
}
