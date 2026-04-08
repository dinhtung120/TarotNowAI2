

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý OTP email.
public class EmailOtpRepository : IEmailOtpRepository
{
    // DbContext thao tác bảng email_otps.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository OTP.
    /// Luồng xử lý: nhận DbContext từ DI để thực hiện truy vấn/lưu OTP.
    /// </summary>
    public EmailOtpRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Thêm bản ghi OTP mới.
    /// Luồng xử lý: add OTP vào DbSet và save ngay để OTP có hiệu lực tức thời.
    /// </summary>
    public async Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default)
    {
        await _dbContext.EmailOtps.AddAsync(otp, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy OTP active mới nhất theo user và loại OTP.
    /// Luồng xử lý: lọc theo user/type, loại bản ghi đã dùng hoặc hết hạn, rồi lấy record CreatedAt mới nhất.
    /// </summary>
    public async Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default)
    {
        return await _dbContext.EmailOtps
            .Where(o => o.UserId == userId && o.Type == type && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
        // Điều kiện ExpiresAt > UtcNow bảo đảm OTP chỉ dùng trong khung thời gian hợp lệ.
    }

    /// <summary>
    /// Cập nhật trạng thái OTP (ví dụ đánh dấu đã dùng).
    /// Luồng xử lý: update entity và persist thay đổi để chặn tái sử dụng OTP.
    /// </summary>
    public async Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default)
    {
        _dbContext.EmailOtps.Update(otp);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
