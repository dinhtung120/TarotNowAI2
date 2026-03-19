/*
 * ===================================================================
 * FILE: RevokeTokenCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.RevokeToken
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thực thi hành động Hủy Token khi đăng xuất.
 *
 * IDEMPOTENT BEHAVIOR (Tính Lũy Đẳng):
 *   Nếu người dùng Đăng xuất 2 lần liên tục với cùng 1 Token đã hết hạn/Đã xóa, 
 *   API này KHÔNG ĐƯỢC QUĂNG LỖI. Vẫn trả về True (thành công) như thường.
 *   Xoá 1 thứ đã biến mất cõi đời, kết quả = Cáo chung rồi! Thả lỏng cho UI Frontend.
 * ===================================================================
 */

using MediatR;
using TarotNow.Domain.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

/// <summary>
/// Chuyên Xoá dổ cái Refresh Token Session. 
/// Gúp phòng vệ thiết bị trước khi bán/cho tặng.
/// </summary>
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        // Nhánh 1: Logout Khắp Vũ Trụ (RevokeAll). Thường kích hoạt khi Bị Hack / Đổi Pass.
        if (request.RevokeAll && request.UserId.HasValue)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId.Value, cancellationToken);
            return true;
        }

        // Nhánh 2: Logout 1 Thiết bị cục bộ.
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw new DomainException("INVALID_TOKEN", "Token is required for revocation.");
        }

        // Tìm kiếm gốc rễ cái dòng Cookie.
        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        
        // Nguyên tắc Lũy đẳng: Không thấy thì coi như đã Xoá. (Trả về false cho log, nhưng API 200 OK)
        if (tokenEntity == null) return false;

        // An ninh: Token string phải đúng 100%.
        if (!tokenEntity.MatchesToken(request.Token)) return false;
        
        // Đánh dấu Xoá Sổ chứ không xoá ROW cứng.
        // Giữ ROW lại để Audit Log (Biết thiết bị nào đã log in ở đâu, mấy giờ log out).
        if (!tokenEntity.IsRevoked)
        {
            tokenEntity.Revoke(); // Set IsRevoked = True, RevokedAt = Hôm nay
            await _refreshTokenRepository.UpdateAsync(tokenEntity, cancellationToken);
        }

        return true;
    }
}
