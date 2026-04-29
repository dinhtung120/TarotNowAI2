

using AutoMapper;
using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

// Handler chính cho luồng refresh token và rotate phiên đăng nhập.
public partial class RefreshTokenCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RefreshTokenCommandHandlerRequestedDomainEvent>
{
    private const int MaxRotateLockRetries = 3;

    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly IMapper _mapper;

    /// <summary>
    /// Khởi tạo handler refresh token.
    /// Luồng xử lý: nhận các repository/service cần để validate token, rotate token và phát access token mới.
    /// </summary>
    public RefreshTokenCommandHandlerRequestedDomainEventHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IUserRepository userRepository,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings,
        IDomainEventPublisher domainEventPublisher,
        IMapper mapper,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _domainEventPublisher = domainEventPublisher;
        _mapper = mapper;
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

        var user = await LoadActiveUserAsync(currentToken.UserId, cancellationToken);
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

    protected override async Task HandleDomainEventAsync(
        RefreshTokenCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }

    protected override async Task HandleAlreadyProcessedDomainEventAsync(
        RefreshTokenCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // Inline idempotency hit vẫn cần trả lại payload refresh để controller set cookie ổn định.
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
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
            new RefreshTokenRotatedDomainEvent
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
        return new RefreshTokenResult
        {
            Response = response,
            NewRefreshToken = rotateResult.NewRawToken,
            IsIdempotent = rotateResult.IsIdempotent,
            RefreshTokenExpiresAtUtc = rotateResult.NewTokenExpiresAtUtc
                ?? DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays)
        };
    }
}
