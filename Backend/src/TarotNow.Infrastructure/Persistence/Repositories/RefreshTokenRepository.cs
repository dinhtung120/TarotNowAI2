

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý refresh token người dùng.
public class RefreshTokenRepository : IRefreshTokenRepository
{
    // DbContext thao tác bảng refresh_tokens.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository refresh token.
    /// Luồng xử lý: nhận DbContext từ DI để truy vấn/lưu trạng thái token.
    /// </summary>
    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Lấy refresh token theo chuỗi token đầu vào.
    /// Luồng xử lý: chuẩn hóa token, thử hash rồi query hỗ trợ cả dữ liệu hash và dữ liệu legacy chưa hash.
    /// </summary>
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        // Edge case: token rỗng thì trả null sớm để tránh query không cần thiết.

        var normalizedToken = token.Trim();
        var hashedToken = RefreshToken.HashToken(normalizedToken);

        return await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == hashedToken || rt.Token == normalizedToken, cancellationToken);
        // Điều kiện OR giữ tương thích ngược trong giai đoạn chuyển đổi dữ liệu cũ.
    }

    /// <summary>
    /// Thêm refresh token mới.
    /// Luồng xử lý: add token và save ngay để phiên đăng nhập có hiệu lực.
    /// </summary>
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cập nhật refresh token.
    /// Luồng xử lý: mark modified và persist thay đổi (ví dụ revoke/rotate).
    /// </summary>
    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Thu hồi toàn bộ refresh token active của một user.
    /// Luồng xử lý: lấy danh sách token chưa revoke, gọi Revoke từng token và lưu batch nếu có thay đổi.
    /// </summary>
    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var activeTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.Revoke();
            // Dùng domain behavior để đảm bảo quy tắc revoke thống nhất.
        }

        if (activeTokens.Any())
        {
            _dbContext.RefreshTokens.UpdateRange(activeTokens);
            await _dbContext.SaveChangesAsync(cancellationToken);
            // Chỉ save khi có token active để giảm round-trip DB không cần thiết.
        }
    }
}
