

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.Login;

public partial class LoginCommandHandler : IRequestHandler<LoginCommand, (AuthResponse Response, string RefreshToken)>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;
    
    
    
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginCommandHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        ITokenService tokenService, 
        IJwtTokenSettings jwtTokenSettings,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<(AuthResponse Response, string RefreshToken)> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = EnsureValidCredentials(
            await GetUserByIdentityAsync(request.EmailOrUsername, cancellationToken),
            request.Password);
        EnsureUserCanLogin(user);
        await RehashPasswordIfNeededAsync(user, request.Password, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenString = await CreateRefreshTokenAsync(user, request.ClientIpAddress, cancellationToken);
        var response = BuildAuthResponse(user, accessToken);
        return (response, refreshTokenString);
    }
}
