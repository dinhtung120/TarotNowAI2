

using MediatR;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public partial class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (AuthResponse Response, string NewRefreshToken)>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
    }

    public async Task<(AuthResponse Response, string NewRefreshToken)> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await GetRefreshTokenOrThrowAsync(request.Token, cancellationToken);
        EnsureTokenMatches(token, request.Token);
        await EnsureTokenNotCompromisedAsync(token, cancellationToken);
        EnsureTokenNotExpired(token);
        await RotateTokenAsync(token, cancellationToken);

        var user = EnsureUserIsActive(token.User);
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = await IssueRefreshTokenAsync(user, request.ClientIpAddress, cancellationToken);
        var response = BuildAuthResponse(user, newAccessToken);
        return (response, newRefreshToken);
    }
}
