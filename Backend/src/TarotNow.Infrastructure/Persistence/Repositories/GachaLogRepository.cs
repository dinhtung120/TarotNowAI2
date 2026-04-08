

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository lưu và truy vấn lịch sử quay gacha trên Mongo.
public class GachaLogRepository : IGachaLogRepository
{
    // Mongo context truy cập collection gacha_logs.
    private readonly MongoDbContext _mongoDbContext;

    /// <summary>
    /// Khởi tạo repository log gacha.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác collection log.
    /// </summary>
    public GachaLogRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    /// <summary>
    /// Ghi một bản ghi log quay gacha.
    /// Luồng xử lý: map request sang document và insert vào Mongo để phục vụ audit/history.
    /// </summary>
    public async Task InsertLogAsync(GachaLogInsertRequest request, CancellationToken ct)
    {
        var doc = new GachaLogDocument
        {
            UserId = request.UserId,
            BannerCode = request.BannerCode,
            Rarity = request.Rarity,
            RewardType = request.RewardType,
            RewardValue = request.RewardValue,
            SpentDiamond = request.SpentDiamond,
            WasPity = request.WasPity,
            RngSeed = request.RngSeed,
            CreatedAt = request.CreatedAt
        };

        await _mongoDbContext.GachaLogs.InsertOneAsync(doc, cancellationToken: ct);
        // Ghi log tách biệt khỏi transactional DB để theo dõi hành vi quay theo thời gian thực.
    }

    /// <summary>
    /// Lấy lịch sử quay gần nhất của user.
    /// Luồng xử lý: query theo user, sort mới nhất trước, giới hạn số lượng rồi map sang DTO response.
    /// </summary>
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
            // Map tường minh để tránh phụ thuộc trực tiếp Mongo document vào tầng API contract.
        }

        return result;
    }
}
