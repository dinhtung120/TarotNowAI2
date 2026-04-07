

using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IRefreshTokenRepository
{
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

        Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    
        Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
