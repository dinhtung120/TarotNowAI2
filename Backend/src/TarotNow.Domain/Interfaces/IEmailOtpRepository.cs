using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Interfaces;

public interface IEmailOtpRepository
{
    Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default);
    Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default);
    Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default);
}
