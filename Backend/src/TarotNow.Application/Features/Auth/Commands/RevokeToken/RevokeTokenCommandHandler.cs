

using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

// Handler thu hồi refresh token theo yêu cầu bảo mật phiên đăng nhập.
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    /// <summary>
    /// Khởi tạo handler revoke token.
    /// Luồng xử lý: nhận refresh token repository để thao tác revoke theo token hoặc theo user.
    /// </summary>
    public RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    /// <summary>
    /// Xử lý command revoke token.
    /// Luồng xử lý: ưu tiên nhánh revoke all theo user id, nếu không thì revoke token đơn lẻ.
    /// </summary>
    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        if (request.RevokeAll && request.UserId.HasValue)
        {
            // Nhánh revoke all: thu hồi toàn bộ phiên của user chỉ định.
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId.Value, cancellationToken);
            return true;
        }

        if (string.IsNullOrWhiteSpace(request.Token))
        {
            // Khi không revoke all thì token cụ thể là bắt buộc.
            throw new BusinessRuleException("INVALID_TOKEN", "Token is required for revocation.");
        }

        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (tokenEntity == null)
        {
            // Token không tồn tại: trả false để caller biết không có gì bị revoke.
            return false;
        }

        if (!tokenEntity.MatchesToken(request.Token))
        {
            // Edge case token entity không khớp raw token: coi như revoke thất bại an toàn.
            return false;
        }

        if (!tokenEntity.IsRevoked)
        {
            // Chỉ revoke khi token chưa bị revoke để tránh ghi đè trạng thái không cần thiết.
            tokenEntity.Revoke();
            await _refreshTokenRepository.UpdateAsync(tokenEntity, cancellationToken);
        }

        return true;
    }
}
