using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class EmailOtpRepository : IEmailOtpRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmailOtpRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default)
    {
        await _dbContext.EmailOtps.AddAsync(otp, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default)
    {
        // Lấy OTP mới nhất, chưa được sử dụng, chưa hết hạn theo user và type
        return await _dbContext.EmailOtps
            .Where(o => o.UserId == userId && o.Type == type && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default)
    {
        _dbContext.EmailOtps.Update(otp);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
