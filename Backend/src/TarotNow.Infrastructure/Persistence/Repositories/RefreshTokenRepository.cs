/*
 * FILE: RefreshTokenRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng refresh_tokens (PostgreSQL).
 *   Refresh Token dùng để lấy Access Token mới khi Access Token hết hạn.
 *
 *   CÁC CHỨC NĂNG:
 *   → GetByTokenAsync: tìm token (hash hoặc raw) — kèm Include User entity
 *   → AddAsync: lưu token mới
 *   → UpdateAsync: cập nhật (thường là revoke token cũ)
 *   → RevokeAllByUserIdAsync: thu hồi TẤT CẢ token của 1 User (logout toàn bộ thiết bị)
 *
 *   BẢO MẬT:
 *   → Token được hash (SHA-256) trước khi lưu vào DB.
 *   → GetByTokenAsync tìm bằng CẢ hash lẫn raw (backward compatibility).
 *   → RevokeAll: khi User đổi mật khẩu hoặc phát hiện bị hack → kick hết.
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IRefreshTokenRepository — quản lý Refresh Token (PostgreSQL).
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Tìm Refresh Token theo giá trị token (hash hoặc raw).
    /// Include User → vì khi verify token cần biết User là ai để sinh Access Token mới.
    /// 
    /// Tại sao tìm cả hash lẫn raw?
    /// → Token mới được hash trước khi lưu (bảo mật).
    /// → Token cũ (trước khi thêm tính năng hash) vẫn lưu raw.
    /// → Tìm cả 2 để backward compatible (không break token cũ).
    /// </summary>
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        var normalizedToken = token.Trim();
        // Hash token đang tìm → so sánh với hash trong DB
        var hashedToken = RefreshToken.HashToken(normalizedToken);

        return await _dbContext.RefreshTokens
            .Include(rt => rt.User) // Eager load User entity
            .FirstOrDefaultAsync(rt => rt.Token == hashedToken || rt.Token == normalizedToken, cancellationToken);
    }

    /// <summary>Lưu Refresh Token mới vào DB.</summary>
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Cập nhật Refresh Token (thường là set RevokedAt khi xoay token).</summary>
    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Thu hồi TẤT CẢ Refresh Token active của 1 User.
    /// Khi nào gọi?
    ///   → User đổi mật khẩu → kick hết thiết bị
    ///   → Admin khóa tài khoản → invalidate tất cả session
    ///   → User phát hiện bị hack → logout toàn bộ
    /// 
    /// Chỉ thu hồi token chưa bị revoke (RevokedAt == null).
    /// token.Revoke() là Domain method — set RevokedAt = DateTime.UtcNow.
    /// </summary>
    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Lấy tất cả token active (chưa bị thu hồi) của User
        var activeTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        // Gọi Domain method Revoke() cho từng token
        foreach (var token in activeTokens)
        {
            token.Revoke();
        }

        // Batch update: cập nhật tất cả trong 1 lần SaveChanges
        if (activeTokens.Any())
        {
            _dbContext.RefreshTokens.UpdateRange(activeTokens);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
