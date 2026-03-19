/*
 * FILE: UserConsentRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng user_consents (PostgreSQL).
 *   Ghi nhận User đã đồng ý với tài liệu pháp lý nào (ToS, Privacy Policy, v.v.).
 *   Yêu cầu GDPR/Luật bảo vệ dữ liệu: lưu BẰNG CHỨNG User đã consent.
 *
 *   CÁC CHỨC NĂNG:
 *   → GetConsentAsync: kiểm tra User đã consent version cụ thể chưa
 *   → GetUserConsentsAsync: lấy tất cả consent history của User
 *   → AddAsync: ghi nhận consent mới
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IUserConsentRepository — ghi nhận đồng ý pháp lý (PostgreSQL).
/// </summary>
public class UserConsentRepository : IUserConsentRepository
{
    private readonly ApplicationDbContext _context;

    public UserConsentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Kiểm tra User đã đồng ý với document cụ thể (type + version) chưa.
    /// Ví dụ: GetConsentAsync(userId, "terms_of_service", "v2.1")
    /// → Nếu có record → User đã đồng ý ToS v2.1
    /// → Nếu null → User chưa đồng ý → yêu cầu consent trước khi cho tiếp tục
    /// </summary>
    public async Task<UserConsent?> GetConsentAsync(Guid userId, string documentType, string version, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .FirstOrDefaultAsync(c => c.UserId == userId && c.DocumentType == documentType && c.Version == version, cancellationToken);
    }

    /// <summary>
    /// Lấy TẤT CẢ consent history của User — dùng cho Admin/audit.
    /// Cho biết User đã đồng ý những gì: ToS v1, v2, Privacy Policy v1, v.v.
    /// </summary>
    public async Task<IEnumerable<UserConsent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Ghi nhận consent mới (không thể xóa/sửa — append-only cho audit trail).</summary>
    public async Task AddAsync(UserConsent consent, CancellationToken cancellationToken = default)
    {
        await _context.UserConsents.AddAsync(consent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
