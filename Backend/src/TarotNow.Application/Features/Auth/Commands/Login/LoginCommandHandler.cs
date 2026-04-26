using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.Login;

// Handler chính cho luồng đăng nhập và cấp token phiên.
public partial class LoginCommandExecutor : ICommandExecutionExecutor<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler đăng nhập.
    /// Luồng xử lý: nhận user repo, password hasher, token service, jwt settings và refresh token repo.
    /// </summary>
    public LoginCommandExecutor(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command đăng nhập.
    /// Luồng xử lý: tìm user theo identity, xác thực mật khẩu + trạng thái, rehash nếu cần, cấp access/refresh token.
    /// </summary>
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateCredentialsAsync(request, cancellationToken);
        var sessionContext = await CreateSessionContextAsync(user, request, cancellationToken);
        return await IssueLoginResultAsync(user, request, sessionContext, cancellationToken);
    }
}
