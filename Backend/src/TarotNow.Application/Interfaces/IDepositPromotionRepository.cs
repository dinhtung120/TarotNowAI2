using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IDepositPromotionRepository
{
    Task<DepositPromotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DepositPromotion>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DepositPromotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);
    Task UpdateAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);
    Task DeleteAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);
}
