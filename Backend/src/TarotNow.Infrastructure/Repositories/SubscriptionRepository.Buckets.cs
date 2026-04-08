using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

// Repository xử lý bucket entitlement theo ngày nghiệp vụ để phục vụ consume an toàn.
public partial class SubscriptionRepository
{
    /// <summary>
    /// Thêm danh sách bucket entitlement vào DbContext để lưu cùng transaction hiện tại.
    /// Luồng này giúp giữ tính nhất quán khi khởi tạo quyền lợi sau khi kích hoạt gói.
    /// </summary>
    public Task AddBucketsAsync(List<SubscriptionEntitlementBucket> buckets, CancellationToken ct)
    {
        // Gắn toàn bộ bucket vào ChangeTracker để commit atomically ở tầng gọi.
        _context.SubscriptionEntitlementBuckets.AddRange(buckets);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Lấy các bucket còn khả dụng để consume entitlement trong ngày nghiệp vụ.
    /// Luồng ưu tiên bucket sắp hết hạn trước để giảm rủi ro lãng phí quota.
    /// </summary>
    public async Task<List<SubscriptionEntitlementBucket>> GetBucketsForConsumeAsync(
        Guid userId,
        string entitlementKey,
        DateOnly businessDate,
        CancellationToken ct)
    {
        // Khóa dòng bằng FOR UPDATE để tránh 2 request đồng thời consume trùng quota.
        const string sqlQuery = """
            SELECT * FROM subscription_entitlement_buckets
            WHERE user_id = {0}
              AND entitlement_key = {1}
              AND business_date = {2}
              AND used_today < daily_quota
              AND subscription_end_date > {3}
            FOR UPDATE
            """;

        var now = DateTime.UtcNow;
        var buckets = await _context.SubscriptionEntitlementBuckets
            .FromSqlRaw(sqlQuery, userId, entitlementKey, businessDate, now)
            .ToListAsync(ct);
        // Sau khi khóa và lấy dữ liệu, sắp theo ngày hết hạn để tiêu thụ theo thứ tự an toàn.

        return buckets
            .OrderBy(b => b.SubscriptionEndDate)
            .ThenBy(b => b.UserSubscriptionId)
            .ToList();
    }

    /// <summary>
    /// Tổng hợp số dư entitlement theo từng key cho người dùng.
    /// Luồng gom bucket còn hiệu lực rồi tính tổng quota, đã dùng và còn lại của ngày hiện tại.
    /// </summary>
    public async Task<List<EntitlementBalanceDto>> GetBalanceSummaryAsync(Guid userId, DateOnly businessDate, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        // Chỉ lấy bucket còn hiệu lực để kết quả phản ánh đúng số dư có thể sử dụng.
        var buckets = await _context.SubscriptionEntitlementBuckets
            .AsNoTracking()
            .Where(b => b.UserId == userId && b.SubscriptionEndDate > now)
            .ToListAsync(ct);

        // LINQ này gom theo entitlement key để trả về summary gọn cho API và tránh query lặp.
        return buckets
            .GroupBy(b => b.EntitlementKey)
            .Select(g => new EntitlementBalanceDto(
                g.Key,
                g.Sum(b => b.DailyQuota),
                g.Sum(b => b.BusinessDate == businessDate ? b.UsedToday : 0),
                g.Sum(b => b.BusinessDate == businessDate ? (b.DailyQuota - b.UsedToday) : b.DailyQuota)))
            .ToList();
    }

    /// <summary>
    /// Lấy batch bucket cần reset ngày nghiệp vụ cũ.
    /// Luồng giới hạn kích thước batch để job nền xử lý ổn định và tránh giữ lock quá lâu.
    /// </summary>
    public Task<List<SubscriptionEntitlementBucket>> GetAllBucketsForResetAsync(DateOnly oldBusinessDate, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        // Chỉ reset bucket thuộc ngày cũ nhưng subscription vẫn còn hiệu lực.
        return _context.SubscriptionEntitlementBuckets
            .Where(b => b.BusinessDate < oldBusinessDate && b.SubscriptionEndDate > now)
            .OrderBy(b => b.Id)
            .Take(1000)
            .ToListAsync(ct);
    }
}
