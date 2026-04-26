using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý dữ liệu chấp thuận pháp lý của user.
public class UserConsentRepository : IUserConsentRepository
{
    // DbContext thao tác bảng user_consents.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository user consent.
    /// </summary>
    public UserConsentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy một consent theo user + loại tài liệu + phiên bản.
    /// </summary>
    public Task<UserConsent?> GetConsentAsync(
        Guid userId,
        string documentType,
        string version,
        CancellationToken cancellationToken = default)
    {
        return _context.UserConsents.FirstOrDefaultAsync(
            consent => consent.UserId == userId
                       && consent.DocumentType == documentType
                       && consent.Version == version,
            cancellationToken);
    }

    /// <summary>
    /// Lấy toàn bộ consent của một user.
    /// </summary>
    public async Task<IEnumerable<UserConsent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .Where(consent => consent.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Thêm mới consent theo hướng insert-first duplicate-safe.
    /// Luồng xử lý: dùng ON CONFLICT DO NOTHING để tránh exception làm hỏng transaction hiện tại.
    /// </summary>
    public async Task<bool> TryAddAsync(UserConsent consent, CancellationToken cancellationToken = default)
    {
        var affectedRows = await _context.Database.ExecuteSqlInterpolatedAsync(
            $"""
             INSERT INTO user_consents (
                 id,
                 user_id,
                 document_type,
                 version,
                 consented_at,
                 ip_address,
                 user_agent)
             VALUES (
                 {consent.Id},
                 {consent.UserId},
                 {consent.DocumentType},
                 {consent.Version},
                 {consent.ConsentedAt},
                 {consent.IpAddress},
                 {consent.UserAgent})
             ON CONFLICT (user_id, document_type, version) DO NOTHING
             """,
            cancellationToken);

        return affectedRows > 0;
    }
}
