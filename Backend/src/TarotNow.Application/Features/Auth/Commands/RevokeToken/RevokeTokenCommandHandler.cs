

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
        if (request.RevokeAll && request.UserId.HasValue)
        {
            await HandleRevokeAllAsync(request.UserId.Value, cancellationToken);
            return true;
        }

        var tokenEntity = await LoadTokenToRevokeAsync(request.Token, cancellationToken);
        if (tokenEntity is null)
        {
            return false;
        }

        await RevokeSingleTokenAsync(tokenEntity, cancellationToken);
        await PublishSingleLogoutEventAsync(tokenEntity, cancellationToken);
        return true;
    }

    private async Task HandleRevokeAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.RevokeAllByUserIdAsync(userId, cancellationToken);
        await _authSessionRepository.RevokeAllByUserAsync(userId, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new UserLoggedOutDomainEvent
            {
                UserId = userId,
                RevokeAll = true,
                Reason = RefreshRevocationReasons.ManualRevoke
            },
            cancellationToken);
    }

    private async Task<RefreshTokenEntity?> LoadTokenToRevokeAsync(string? rawToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Token is required for revocation.");
        }

        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(rawToken, cancellationToken);
        if (tokenEntity is null)
        {
            return null;
        }

        return tokenEntity.MatchesToken(rawToken) ? tokenEntity : null;
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
