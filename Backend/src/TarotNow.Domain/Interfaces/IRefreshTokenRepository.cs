using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    
    // Thu hồi hàng loạt token của 1 user khi có dấu hiệu fraud hoặc đổi mật khẩu
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
