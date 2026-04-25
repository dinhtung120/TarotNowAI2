

using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý dữ liệu chấp thuận pháp lý của user.
public class UserConsentRepository : IUserConsentRepository
{
    private const string UserConsentUniqueConstraintSuffix = "userid_documenttype_version";
    // DbContext thao tác bảng user_consents.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository user consent.
    /// Luồng xử lý: nhận DbContext từ DI để thao tác truy vấn/ghi consent.
    /// </summary>
    public UserConsentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy một consent theo user + loại tài liệu + phiên bản.
    /// Luồng xử lý: truy vấn bản ghi đầu tiên khớp bộ khóa nghiệp vụ.
    /// </summary>
    public async Task<UserConsent?> GetConsentAsync(Guid userId, string documentType, string version, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .FirstOrDefaultAsync(c => c.UserId == userId && c.DocumentType == documentType && c.Version == version, cancellationToken);
    }

    /// <summary>
    /// Lấy toàn bộ consent của một user.
    /// Luồng xử lý: filter theo userId và trả danh sách bản ghi.
    /// </summary>
    public async Task<IEnumerable<UserConsent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Thêm mới consent theo hướng insert-first idempotent.
    /// Luồng xử lý: add entity và save ngay; nếu trùng unique key thì trả false.
    /// </summary>
    public async Task<bool> TryAddAsync(UserConsent consent, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.UserConsents.AddAsync(consent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (IsDuplicateConsent(exception))
        {
            _context.Entry(consent).State = EntityState.Detached;
            return false;
        }
    }

    private static bool IsDuplicateConsent(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && (postgresException.ConstraintName?.Replace("_", string.Empty, StringComparison.OrdinalIgnoreCase)
                   .Contains(UserConsentUniqueConstraintSuffix, StringComparison.OrdinalIgnoreCase) ?? false);
    }
}
