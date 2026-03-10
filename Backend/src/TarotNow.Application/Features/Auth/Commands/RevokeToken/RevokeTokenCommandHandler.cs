using MediatR;
using TarotNow.Domain.Exceptions;
using TarotNow.Domain.Interfaces;

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
            // Logout everywhere
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId.Value, cancellationToken);
            return true;
        }

        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw new DomainException("INVALID_TOKEN", "Token is required for revocation.");
        }

        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (tokenEntity == null) return false; // Không tìm thấy thì vẫn trả về false nhưng không throw Exception (idempotent)
        
        if (!tokenEntity.IsRevoked)
        {
            tokenEntity.Revoke();
            await _refreshTokenRepository.UpdateAsync(tokenEntity, cancellationToken);
        }

        return true;
    }
}
