

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

// Repository xử lý dữ liệu banner/item/log thưởng của gacha trên PostgreSQL.
public class GachaRepository : IGachaRepository
{
    // DbContext thao tác các bảng gacha_*.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository gacha.
    /// Luồng xử lý: nhận DbContext từ DI để dùng chung transaction khi quay thưởng.
    /// </summary>
    public GachaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Lấy banner đang hoạt động theo code.
    /// Luồng xử lý: lọc theo code + cờ active + khung thời gian hiệu lực tại thời điểm hiện tại.
    /// </summary>
    public async Task<GachaBanner?> GetActiveBannerAsync(string bannerCode, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.GachaBanners
            .Where(b => b.Code == bannerCode && b.IsActive && b.EffectiveFrom <= now && (b.EffectiveTo == null || b.EffectiveTo > now))
            .FirstOrDefaultAsync(ct);
        // Điều kiện EffectiveTo nullable hỗ trợ banner mở vô thời hạn.
    }

    /// <summary>
    /// Lấy danh sách toàn bộ banner đang hoạt động.
    /// Luồng xử lý: lọc theo active + khoảng hiệu lực theo thời gian UTC.
    /// </summary>
    public async Task<List<GachaBanner>> GetAllActiveBannersAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.GachaBanners
            .Where(b => b.IsActive && b.EffectiveFrom <= now && (b.EffectiveTo == null || b.EffectiveTo > now))
            .ToListAsync(ct);
    }

    /// <summary>
    /// Lấy toàn bộ item thuộc một banner.
    /// Luồng xử lý: filter theo bannerId và trả danh sách item để tính xác suất quay.
    /// </summary>
    public async Task<List<GachaBannerItem>> GetBannerItemsAsync(Guid bannerId, CancellationToken ct)
    {
        return await _dbContext.GachaBannerItems
            .Where(i => i.BannerId == bannerId)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Tính pity count hiện tại của user trên một banner.
    /// Luồng xử lý: tìm lần trúng legendary gần nhất, sau đó đếm số lượt quay kể từ mốc đó.
    /// </summary>
    public async Task<int> GetUserPityCountAsync(Guid userId, Guid bannerId, CancellationToken ct)
    {
        var lastLegendaryLog = await _dbContext.GachaRewardLogs
            .Where(l => l.UserId == userId && l.BannerId == bannerId && l.Rarity == GachaRarity.Legendary)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync(ct);
        // Mốc legendary gần nhất là điểm reset pity theo rule nghiệp vụ.

        var query = _dbContext.GachaRewardLogs
            .Where(l => l.UserId == userId && l.BannerId == bannerId);

        if (lastLegendaryLog != null)
        {
            query = query.Where(l => l.CreatedAt > lastLegendaryLog.CreatedAt);
            // Chỉ đếm các lượt quay sau lần legendary gần nhất để phản ánh pity hiện tại.
        }

        return await query.CountAsync(ct);
    }

    /// <summary>
    /// Ghi log phần thưởng quay gacha.
    /// Luồng xử lý: add log vào DbSet, save và trả lại entity đã lưu.
    /// </summary>
    public async Task<GachaRewardLog> LogRewardAsync(GachaRewardLog log, CancellationToken ct)
    {
        _dbContext.GachaRewardLogs.Add(log);
        await _dbContext.SaveChangesAsync(ct);
        return log;
    }

    /// <summary>
    /// Kiểm tra idempotency key đã tồn tại hay chưa.
    /// Luồng xử lý: dùng AnyAsync để kiểm tra nhanh duplicate request quay thưởng.
    /// </summary>
    public async Task<bool> IdempotencyKeyExistsAsync(string key, CancellationToken ct)
    {
        return await _dbContext.GachaRewardLogs.AnyAsync(l => l.IdempotencyKey == key, ct);
    }

    /// <summary>
    /// Lấy các log reward theo idempotency key.
    /// Luồng xử lý: truy vấn và sắp theo CreatedAt tăng dần để tái dựng đúng thứ tự xử lý.
    /// </summary>
    public async Task<List<GachaRewardLog>> GetRewardLogsByIdempotencyKeyAsync(string key, CancellationToken ct)
    {
        return await _dbContext.GachaRewardLogs
            .Where(x => x.IdempotencyKey == key)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}
