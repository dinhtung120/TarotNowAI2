

using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

// Handler chính cho luồng refresh token và rotate phiên đăng nhập.
public partial class RefreshTokenCommandExecutor : ICommandExecutionExecutor<RefreshTokenCommand, RefreshTokenResult>
{
    private const int MaxRotateLockRetries = 3;

    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler refresh token.
    /// Luồng xử lý: nhận các repository/service cần để validate token, rotate token và phát access token mới.
    /// </summary>
    public RefreshTokenCommandExecutor(
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings,
        IDomainEventPublisher domainEventPublisher)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command refresh token.
    /// Luồng xử lý: kiểm tra token tồn tại/hợp lệ/chưa bị compromise/chưa hết hạn, rotate token cũ, cấp token mới.
    /// </summary>
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await EnsureLegacyTokenSessionBindingAsync(request, cancellationToken);
        var rotateResult = await RotateRefreshTokenAsync(request, cancellationToken);
        var currentToken = await ValidateRotationResultAsync(rotateResult, request, cancellationToken);

        var user = EnsureUserIsActive(currentToken.User);
        var sessionId = currentToken.SessionId;
        await EnsureSessionIsActiveAsync(sessionId, cancellationToken);
        await TouchSessionAsync(sessionId, request, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user, sessionId, out _, out var jti);
        var response = BuildAuthResponse(user, accessToken);
        await PublishTokenRefreshedEventIfNeededAsync(
            rotateResult,
            currentToken,
            request,
            jti,
            cancellationToken);

        return BuildRefreshResult(response, rotateResult);
    }

    private async Task<RefreshRotateResult> RotateRefreshTokenAsync(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var nextRefreshToken = BuildDeterministicRefreshToken(request);
        for (var attempt = 1; attempt <= MaxRotateLockRetries; attempt++)
        {
            var rotateRequest = new RefreshRotateRequest
            {
                RawToken = request.Token,
                NewRawToken = nextRefreshToken,
                NewExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays),
                IpAddress = request.ClientIpAddress,
                DeviceId = request.DeviceId,
                UserAgentHash = request.UserAgentHash,
                IdempotencyKey = request.IdempotencyKey
            };

            var rotateResult = await _refreshTokenRepository.RotateAsync(rotateRequest, cancellationToken);
            if (rotateResult.Status != RefreshRotateStatus.Locked || attempt == MaxRotateLockRetries)
            {
                return rotateResult;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(40 * attempt), cancellationToken);
        }

        return RefreshRotateResult.Locked();
    }

    private static string BuildDeterministicRefreshToken(RefreshTokenCommand request)
    {
        var material = string.Concat(
            request.Token.Trim(),
            "|",
            request.IdempotencyKey.Trim(),
            "|",
            request.DeviceId.Trim(),
            "|",
            request.UserAgentHash.Trim());
        var bytes = System.Security.Cryptography.SHA512.HashData(System.Text.Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private async Task<RefreshTokenEntity> ValidateRotationResultAsync(
        RefreshRotateResult rotateResult,
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (rotateResult.Status == RefreshRotateStatus.ReplayDetected)
        {
            await HandleReplayDetectedAsync(rotateResult, request, cancellationToken);
            throw new BusinessRuleException(AuthErrorCodes.TokenReplay, "Refresh token replay detected. Please log in again.");
        }

        if (rotateResult.Status == RefreshRotateStatus.Expired)
        {
            throw new BusinessRuleException(AuthErrorCodes.TokenExpired, "Refresh token has expired. Please log in again.");
        }

        if (rotateResult.Status == RefreshRotateStatus.Locked)
        {
            throw new BusinessRuleException(
                AuthErrorCodes.RateLimited,
                "Refresh token request is being processed. Please retry shortly.");
        }

        if (rotateResult.Status == RefreshRotateStatus.InvalidToken)
        {
            throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Refresh token is invalid.");
        }

        return rotateResult.CurrentToken
            ?? throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Refresh token is invalid.");
    }

    private async Task EnsureSessionIsActiveAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        var activeSession = await _authSessionRepository.GetActiveAsync(sessionId, cancellationToken);
        if (activeSession is null)
        {
            throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Session is no longer active.");
        }
    }

    private async Task PublishTokenRefreshedEventIfNeededAsync(
        RefreshRotateResult rotateResult,
        RefreshTokenEntity currentToken,
        RefreshTokenCommand request,
        string accessTokenJti,
        CancellationToken cancellationToken)
    {
        if (rotateResult.IsIdempotent || rotateResult.NewToken is null)
        {
            return;
        }

        await _domainEventPublisher.PublishAsync(
            new TokenRefreshedDomainEvent
            {
                UserId = currentToken.UserId,
                SessionId = currentToken.SessionId,
                OldTokenId = currentToken.Id,
                NewTokenId = rotateResult.NewToken.Id,
                AccessTokenJti = accessTokenJti,
                DeviceId = request.DeviceId,
                IpHash = HashValue(request.ClientIpAddress),
                UserAgentHash = request.UserAgentHash
            },
            cancellationToken);
    }

    private RefreshTokenResult BuildRefreshResult(AuthResponse response, RefreshRotateResult rotateResult)
    {
        var refreshTokenExpiresAtUtc = rotateResult.NewTokenExpiresAtUtc
            ?? DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays);
        return new RefreshTokenResult
        {
            Response = response,
            NewRefreshToken = rotateResult.NewRawToken,
            IsIdempotent = rotateResult.IsIdempotent,
            RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc
        };
    }
}
