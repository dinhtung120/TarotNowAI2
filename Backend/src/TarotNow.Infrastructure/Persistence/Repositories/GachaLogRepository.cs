/*
 * ===================================================================
 * FILE: GachaLogRepository.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Repositories
 * ===================================================================
 */

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class GachaLogRepository : IGachaLogRepository
{
    private readonly MongoDbContext _mongoDbContext;

    public GachaLogRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public async Task InsertLogAsync(Guid userId, string bannerCode, string rarity, string rewardType, string rewardValue, long spentDiamond, bool wasPity, string? rngSeed, DateTime createdAt, CancellationToken ct)
    {
        var doc = new GachaLogDocument
        {
            UserId = userId,
            BannerCode = bannerCode,
            Rarity = rarity,
            RewardType = rewardType,
            RewardValue = rewardValue,
            SpentDiamond = spentDiamond,
            WasPity = wasPity,
            RngSeed = rngSeed,
            CreatedAt = createdAt
        };

        await _mongoDbContext.GachaLogs.InsertOneAsync(doc, cancellationToken: ct);
    }

    public async Task<List<TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto>> GetUserLogsAsync(Guid userId, int limit, CancellationToken ct)
    {
        var logs = await _mongoDbContext.GachaLogs
            .Find(x => x.UserId == userId)
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .ToListAsync(ct);

        var result = new List<TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto>();
        foreach (var l in logs)
        {
            result.Add(new TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto
            {
                BannerCode = l.BannerCode,
                Rarity = l.Rarity,
                RewardType = l.RewardType,
                RewardValue = l.RewardValue,
                SpentDiamond = l.SpentDiamond,
                WasPityTriggered = l.WasPity,
                CreatedAt = l.CreatedAt
            });
        }
        return result;
    }
}
