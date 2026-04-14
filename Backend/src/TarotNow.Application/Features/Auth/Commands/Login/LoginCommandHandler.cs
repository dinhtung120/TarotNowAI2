

using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.Login;

// Handler chính cho luồng đăng nhập và cấp token phiên.
public partial class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ICacheService _cacheService;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler đăng nhập.
    /// Luồng xử lý: nhận user repo, password hasher, token service, jwt settings và refresh token repo.
    /// </summary>
    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        ICacheService cacheService,
        IDomainEventPublisher domainEventPublisher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _cacheService = cacheService;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command đăng nhập.
    /// Luồng xử lý: tìm user theo identity, xác thực mật khẩu + trạng thái, rehash nếu cần, cấp access/refresh token.
    /// </summary>
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await EnsureLoginThrottleNotExceededAsync(request, cancellationToken);

        var user = await GetUserByIdentityAsync(request.EmailOrUsername, cancellationToken);
        if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            await _domainEventPublisher.PublishAsync(
                BuildLoginFailedEvent(request, AuthErrorCodes.Unauthorized),
                cancellationToken);
            throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Invalid email/username or password.");
        }

        try
        {
            EnsureUserCanLogin(user);
        }
        catch (BusinessRuleException ex)
        {
            await _domainEventPublisher.PublishAsync(
                BuildLoginFailedEvent(request, ex.ErrorCode),
                cancellationToken);
            throw;
        }
        // Nâng cấp hash mật khẩu ngầm khi thuật toán/hash parameters đã thay đổi.
        await RehashPasswordIfNeededAsync(user, request.Password, cancellationToken);

        var ipHash = HashValue(request.ClientIpAddress);
        var session = await _authSessionRepository.CreateAsync(
            user.Id,
            request.DeviceId,
            request.UserAgentHash,
            ipHash,
            cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user, session.Id, out _, out var accessTokenJti);
        // Tạo refresh token mới gắn với IP client để phục vụ vòng đời phiên.
        var refreshTokenString = await CreateRefreshTokenAsync(user, session.Id, request, cancellationToken);
        var response = BuildAuthResponse(user, accessToken);

        await ClearLoginFailureCountersAsync(request, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new UserLoggedInDomainEvent
            {
                UserId = user.Id,
                SessionId = session.Id,
                DeviceId = request.DeviceId,
                UserAgentHash = request.UserAgentHash,
                IpHash = ipHash,
                AccessTokenJti = accessTokenJti
            },
            cancellationToken);

        return new LoginResult
        {
            Response = response,
            RefreshToken = refreshTokenString
        };
    }
}
