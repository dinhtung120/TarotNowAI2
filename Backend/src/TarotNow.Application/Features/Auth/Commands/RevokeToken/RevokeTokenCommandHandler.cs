

using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        
        if (request.RevokeAll && request.UserId.HasValue)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId.Value, cancellationToken);
            return true;
        }

        
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw new BusinessRuleException("INVALID_TOKEN", "Token is required for revocation.");
        }

        
        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        
        
        if (tokenEntity == null) return false;

        
        if (!tokenEntity.MatchesToken(request.Token)) return false;
        
        
        
        if (!tokenEntity.IsRevoked)
        {
            tokenEntity.Revoke(); 
            await _refreshTokenRepository.UpdateAsync(tokenEntity, cancellationToken);
        }

        return true;
    }
}
