/*
 * FILE: EmailOtpRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng email_otps (PostgreSQL).
 *   Mã OTP gửi qua email để xác thực hành động (đăng ký, quên mật khẩu, v.v.).
 *
 *   CÁC CHỨC NĂNG:
 *   → AddAsync: tạo mới mã OTP
 *   → GetLatestActiveOtpAsync: lấy OTP mới nhất còn hiệu lực (chưa dùng, chưa hết hạn)
 *   → UpdateAsync: đánh dấu OTP đã dùng (IsUsed = true)
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IEmailOtpRepository — truy cập bảng email_otps (PostgreSQL).
/// </summary>
public class EmailOtpRepository : IEmailOtpRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmailOtpRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>Thêm mã OTP mới vào DB (gửi email trước, lưu DB sau).</summary>
    public async Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default)
    {
        await _dbContext.EmailOtps.AddAsync(otp, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy OTP MỚI NHẤT còn hiệu lực cho 1 User và 1 loại (type).
    /// Điều kiện: cùng UserId, cùng Type, chưa dùng (IsUsed=false), chưa hết hạn.
    /// OrderByDescending(CreatedAt): lấy cái mới nhất (vì User có thể gửi lại OTP nhiều lần).
    /// Composite Index (UserId, Type, IsUsed, ExpiresAt) giúp query này cực nhanh.
    /// </summary>
    public async Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default)
    {
        return await _dbContext.EmailOtps
            .Where(o => o.UserId == userId && o.Type == type && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>Cập nhật OTP (thường là set IsUsed = true sau khi xác thực thành công).</summary>
    public async Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default)
    {
        _dbContext.EmailOtps.Update(otp);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
