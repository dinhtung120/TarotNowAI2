/*
 * ===================================================================
 * FILE: SubscriptionRepository.cs
 * NAMESPACE: TarotNow.Infrastructure.Repositories
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao tiếp trực tiếp với EF Core để kéo nhả dữ liệu Gói Đăng Ký và Số dư Quyền Lợi.
 *   Xử lý việc khóa dòng SQL raw (`FOR UPDATE`) để ngăn ngừa lấp lấp Transaction khi trừ số.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // 1. QUẢN LÝ GÓI ĐĂNG KÝ (PLANS)
    // ==========================================

    public Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId, CancellationToken ct)
    {
        return _context.SubscriptionPlans
            .FirstOrDefaultAsync(p => p.Id == planId, ct);
    }

    public Task<List<SubscriptionPlan>> GetActivePlansAsync(CancellationToken ct)
    {
        return _context.SubscriptionPlans
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync(ct);
    }

    public Task AddPlanAsync(SubscriptionPlan plan, CancellationToken ct)
    {
        _context.SubscriptionPlans.Add(plan);
        return Task.CompletedTask;
    }

    public Task UpdatePlanAsync(SubscriptionPlan plan, CancellationToken ct)
    {
        _context.SubscriptionPlans.Update(plan);
        return Task.CompletedTask;
    }

    // ==========================================
    // 2. BIÊN LAI SỞ HỮU GÓI CỦA KHÁCH (USER SUBSCRIPTIONS)
    // ==========================================

    public Task<UserSubscription?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct)
    {
        // Kiểm Tra Lệnh Mua Chống Trùng (Idempotency)
        return _context.UserSubscriptions
            .FirstOrDefaultAsync(s => s.IdempotencyKey == idempotencyKey, ct);
    }

    public Task<List<UserSubscription>> GetActiveSubscriptionsAsync(Guid userId, CancellationToken ct)
    {
        // Lấy những Gói đang gắn cờ Active + Chưa lết tới vạch Hết Hạn
        var now = DateTime.UtcNow;
        return _context.UserSubscriptions
            .Include(s => s.Plan)
            .Where(s => s.UserId == userId 
                     && s.Status == SubscriptionStatus.Active 
                     && s.EndDate > now)
            .ToListAsync(ct);
    }

    public Task<List<UserSubscription>> GetExpiredSubscriptionsToProcessAsync(DateTime cutoff, CancellationToken ct)
    {
        // Tìm nhanh đống hồ sơ Active nhưng Lố Vạch EndDate. 
        // Lấy 1 lượng nhỏ 50 dòng chạy cuốn chiếu (Paging/Batch) đỡ Kẹt Ram Hệ Thống
        return _context.UserSubscriptions
            .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= cutoff)
            .OrderBy(s => s.EndDate)
            .Take(50)
            .ToListAsync(ct);
    }

    public Task AddSubscriptionAsync(UserSubscription subscription, CancellationToken ct)
    {
        _context.UserSubscriptions.Add(subscription);
        return Task.CompletedTask;
    }

    public Task UpdateSubscriptionAsync(UserSubscription subscription, CancellationToken ct)
    {
        _context.UserSubscriptions.Update(subscription);
        return Task.CompletedTask;
    }

    // ==========================================
    // 3. RỔ QUYỀN LỢI (ENTITLEMENT BUCKETS)
    // ==========================================

    public Task AddBucketsAsync(List<SubscriptionEntitlementBucket> buckets, CancellationToken ct)
    {
        _context.SubscriptionEntitlementBuckets.AddRange(buckets);
        return Task.CompletedTask;
    }

    public async Task<List<SubscriptionEntitlementBucket>> GetBucketsForConsumeAsync(
        Guid userId, 
        string entitlementKey, 
        DateOnly businessDate, 
        CancellationToken ct)
    {
        /*
         * [FUTURE-3 FIX + ĐIỂM NHẠY CẢM MẢNG FINANCE]:
         * Cần Chặn Hành Động "Bấm Liên Tục Ngón Tay Nhanh Gấp Đôi Tốc Độ Mạng Xử Lý Của Server".
         * Đã thêm cổng bảo vệ KHÔNG cho consume bucket từ gói đã hết hạn:
         *   - subscription_end_date > NOW() → chỉ lấy bucket của gói còn hạn
         *   - business_date = {2} → chỉ lấy bucket đã được reset cho ngày hôm nay
         */
        
        string sqlQuery = @"
            SELECT * FROM subscription_entitlement_buckets
            WHERE user_id = {0} 
              AND entitlement_key = {1}
              AND business_date = {2}
              AND used_today < daily_quota
              AND subscription_end_date > {3}
            FOR UPDATE";

        var now = DateTime.UtcNow;
        var buckets = await _context.SubscriptionEntitlementBuckets
            .FromSqlRaw(sqlQuery, userId, entitlementKey, businessDate, now)
            .ToListAsync(ct);

        // Chốt Sorting Tiêu Chuẩn Theo Architecture BR-16
        // - Earliest-Expiry-First: Cái Nào Gần Hết Hạn Kéo Ra Đốt Cho Người Ta Nhờ.
        // - Tie Breaker: ID Gói Cuối Cùng (Tránh Lật Tung Cặp Bằng).
        return buckets
            .OrderBy(b => b.SubscriptionEndDate)
            .ThenBy(b => b.UserSubscriptionId)
            .ToList();
    }

    public async Task<List<EntitlementBalanceDto>> GetBalanceSummaryAsync(
        Guid userId, 
        DateOnly businessDate, 
        CancellationToken ct)
    {
        /*
         * FUTURE-2 FIX: Nếu EntitlementDailyResetJob chưa kịp chạy (delay 15 phút),
         * bucket sẽ giữ BusinessDate cũ → query chỉ lấy bucket của ngày hôm nay sẽ trả rỗng.
         * FIX: Lấy TẤT CẢ bucket của user (còn hạn), nếu BusinessDate < today thì coi như UsedToday = 0.
         */
        var now = DateTime.UtcNow;
        var buckets = await _context.SubscriptionEntitlementBuckets
            .AsNoTracking()
            .Where(b => b.UserId == userId && b.SubscriptionEndDate > now)
            .ToListAsync(ct);

        var summary = buckets
            .GroupBy(b => b.EntitlementKey)
            .Select(g => new EntitlementBalanceDto(
                g.Key,
                g.Sum(b => b.DailyQuota),
                // Nếu BusinessDate < today, coi như chưa dùng lượt nào (UsedToday = 0)
                g.Sum(b => b.BusinessDate == businessDate ? b.UsedToday : 0),
                g.Sum(b => b.BusinessDate == businessDate 
                    ? (b.DailyQuota - b.UsedToday) 
                    : b.DailyQuota)
            ))
            .ToList();

        return summary;
    }

    public async Task<List<SubscriptionEntitlementBucket>> GetAllBucketsForResetAsync(
        DateOnly oldBusinessDate, 
        CancellationToken ct)
    {
        /*
         * FUTURE-4 FIX: Chỉ reset bucket của gói còn hạn (Active).
         * Lọc bỏ bucket của gói đã expired/cancelled để tiết kiệm IO database.
         */
        var now = DateTime.UtcNow;
        return await _context.SubscriptionEntitlementBuckets
            .Where(b => b.BusinessDate < oldBusinessDate && b.SubscriptionEndDate > now)
            .OrderBy(b => b.Id)
            .Take(1000)
            .ToListAsync(ct);
    }

    // ==========================================
    // 4. LUẬT QUY ĐỔI CHÉO MẶC ĐỊNH
    // ==========================================

    public Task<List<EntitlementMappingRule>> GetEnabledMappingRulesAsync(
        string sourceKey, 
        CancellationToken ct)
    {
        return _context.EntitlementMappingRules
            .Where(m => m.SourceKey == sourceKey && m.IsEnabled)
            .ToListAsync(ct);
    }

    // ==========================================
    // 5. LƯU LOG BẰNG CHỨNG (CONSUME LOGS)
    // ==========================================

    public Task AddConsumeLogAsync(EntitlementConsume consume, CancellationToken ct)
    {
        _context.EntitlementConsumes.Add(consume);
        return Task.CompletedTask;
    }

    public Task<bool> ConsumeLogExistsAsync(string idempotencyKey, CancellationToken ct)
    {
        return _context.EntitlementConsumes
            .AnyAsync(c => c.IdempotencyKey == idempotencyKey, ct);
    }

    // ==========================================
    // 6. DB COMMIT LỆNH
    // ==========================================

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
