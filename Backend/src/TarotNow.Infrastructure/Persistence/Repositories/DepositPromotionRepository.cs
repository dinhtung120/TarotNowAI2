

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class DepositPromotionRepository : IDepositPromotionRepository
{
    private readonly ApplicationDbContext _context;

    public DepositPromotionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

        public async Task<DepositPromotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

        public async Task<IEnumerable<DepositPromotion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions
            .OrderByDescending(p => p.MinAmountVnd)
            .ToListAsync(cancellationToken);
    }

        public async Task<IEnumerable<DepositPromotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.MinAmountVnd)
            .ToListAsync(cancellationToken);
    }

        public async Task AddAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        await _context.DepositPromotions.AddAsync(promotion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

        public async Task UpdateAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        _context.DepositPromotions.Update(promotion);
        await _context.SaveChangesAsync(cancellationToken);
    }

        public async Task DeleteAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        _context.DepositPromotions.Remove(promotion);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
