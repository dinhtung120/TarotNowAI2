

using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IEmailOtpRepository
{
        Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default);

        Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default);

        Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default);
}
