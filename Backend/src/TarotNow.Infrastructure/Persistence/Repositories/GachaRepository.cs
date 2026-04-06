/*
 * ===================================================================
 * FILE: GachaRepository.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Repositories
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class GachaRepository : IGachaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GachaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GachaBanner?> GetActiveBannerAsync(string bannerCode, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.GachaBanners
            .Where(b => b.Code == bannerCode && b.IsActive && b.EffectiveFrom <= now && (b.EffectiveTo == null || b.EffectiveTo > now))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<GachaBanner>> GetAllActiveBannersAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.GachaBanners
            .Where(b => b.IsActive && b.EffectiveFrom <= now && (b.EffectiveTo == null || b.EffectiveTo > now))
            .ToListAsync(ct);
    }

    public async Task<List<GachaBannerItem>> GetBannerItemsAsync(Guid bannerId, CancellationToken ct)
    {
        return await _dbContext.GachaBannerItems
            .Where(i => i.BannerId == bannerId)
            .ToListAsync(ct);
    }

    public async Task<int> GetUserPityCountAsync(Guid userId, Guid bannerId, CancellationToken ct)
    {
        // Lấy lần ra Legendary gần nhất
        var lastLegendaryLog = await _dbContext.GachaRewardLogs
            .Where(l => l.UserId == userId && l.BannerId == bannerId && l.Rarity == GachaRarity.Legendary)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync(ct);

        var query = _dbContext.GachaRewardLogs
            .Where(l => l.UserId == userId && l.BannerId == bannerId);

        if (lastLegendaryLog != null)
        {
            // Chỉ đếm những lần quay sau lần ra Legendary gần nhất
            query = query.Where(l => l.CreatedAt > lastLegendaryLog.CreatedAt);
        }

        return await query.CountAsync(ct);
    }

    public async Task<GachaRewardLog> LogRewardAsync(GachaRewardLog log, CancellationToken ct)
    {
        _dbContext.GachaRewardLogs.Add(log);
        await _dbContext.SaveChangesAsync(ct);
        return log;
    }

    public async Task<bool> IdempotencyKeyExistsAsync(string key, CancellationToken ct)
    {
        return await _dbContext.GachaRewardLogs.AnyAsync(l => l.IdempotencyKey == key, ct);
    }

    public async Task<List<GachaRewardLog>> GetRewardLogsByIdempotencyKeyAsync(string key, CancellationToken ct)
    {
        return await _dbContext.GachaRewardLogs
            .Where(x => x.IdempotencyKey == key)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}
