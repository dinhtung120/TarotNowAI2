

using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

// Handler thu hồi refresh token theo yêu cầu bảo mật phiên đăng nhập.
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler revoke token.
    /// Luồng xử lý: nhận refresh token repository để thao tác revoke theo token hoặc theo user.
    /// </summary>
    public RevokeTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command revoke token.
    /// Luồng xử lý: ưu tiên nhánh revoke all theo user id, nếu không thì revoke token đơn lẻ.
    /// </summary>
    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        if (request.RevokeAll)
        {
            var tokenEntity = await LoadTokenToRevokeAsync(
                request.Token,
                tokenRequired: false,
                cancellationToken);
            var userId = ResolveRevokeAllUserId(request.UserId, tokenEntity);
            EnsureRevokeAllIdentityConsistency(request.UserId, tokenEntity);

            await HandleRevokeAllAsync(userId, cancellationToken);
            return true;
        }

        var singleTokenEntity = await LoadTokenToRevokeAsync(
            request.Token,
            tokenRequired: true,
            cancellationToken);
        if (singleTokenEntity is null)
        {
            return false;
        }

        await RevokeSingleTokenAsync(singleTokenEntity, cancellationToken);
        await PublishSingleLogoutEventAsync(singleTokenEntity, cancellationToken);
        return true;
    }

    private async Task HandleRevokeAllAsync(Guid userId, CancellationToken cancellationToken)
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
                Reason = RefreshRevocationReasons.ManualRevoke
            },
            cancellationToken);
    }

    private static Guid ResolveRevokeAllUserId(Guid? requestedUserId, RefreshTokenEntity? tokenEntity)
    {
        if (requestedUserId is Guid userId && userId != Guid.Empty)
        {
            return userId;
        }

        if (tokenEntity is not null && tokenEntity.UserId != Guid.Empty)
        {
            return tokenEntity.UserId;
        }

        throw new BusinessRuleException(
            AuthErrorCodes.Unauthorized,
            "Unable to resolve user identity for revoke-all operation.");
    }

    private static void EnsureRevokeAllIdentityConsistency(Guid? requestedUserId, RefreshTokenEntity? tokenEntity)
    {
        if (requestedUserId is not Guid userId || userId == Guid.Empty || tokenEntity is null)
        {
            return;
        }

        if (tokenEntity.UserId != userId)
        {
            throw new BusinessRuleException(
                AuthErrorCodes.Unauthorized,
                "Refresh token does not belong to the authenticated user.");
        }
    }

    private async Task<RefreshTokenEntity?> LoadTokenToRevokeAsync(
        string? rawToken,
        bool tokenRequired,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            if (tokenRequired)
            {
                throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Token is required for revocation.");
            }

            return null;
        }

        var normalizedToken = rawToken.Trim();
        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(normalizedToken, cancellationToken);
        if (tokenEntity is null)
        {
            return null;
        }

        return tokenEntity.MatchesToken(normalizedToken) ? tokenEntity : null;
    }

    private async Task RevokeSingleTokenAsync(RefreshTokenEntity tokenEntity, CancellationToken cancellationToken)
    {
        if (!tokenEntity.IsRevoked)
        {
            tokenEntity.Revoke(RefreshRevocationReasons.ManualRevoke);
            await _refreshTokenRepository.UpdateAsync(tokenEntity, cancellationToken);
        }

        if (tokenEntity.SessionId == Guid.Empty)
        {
            if (tokenEntity.FamilyId != Guid.Empty)
            {
                await _refreshTokenRepository.RevokeFamilyAsync(
                    tokenEntity.FamilyId,
                    RefreshRevocationReasons.ManualRevoke,
                    cancellationToken);
            }

            return;
        }

        await _refreshTokenRepository.RevokeSessionAsync(
            tokenEntity.SessionId,
            RefreshRevocationReasons.ManualRevoke,
            cancellationToken);
        await _authSessionRepository.RevokeAsync(tokenEntity.SessionId, cancellationToken);
    }

    private async Task PublishSingleLogoutEventAsync(RefreshTokenEntity tokenEntity, CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new UserLoggedOutDomainEvent
            {
                UserId = tokenEntity.UserId,
                SessionId = tokenEntity.SessionId == Guid.Empty ? null : tokenEntity.SessionId,
                RevokeAll = false,
                Reason = RefreshRevocationReasons.ManualRevoke
            },
            cancellationToken);
    }
}
