using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    /// <summary>
    /// Tìm subscription theo idempotency key của giao dịch tạo gói.
    /// Luồng này giúp chống tạo trùng subscription khi request bị retry.
    /// </summary>
    public Task<UserSubscription?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct)
    {
        return _context.UserSubscriptions.FirstOrDefaultAsync(s => s.IdempotencyKey == idempotencyKey, ct);
    }

    /// <summary>
    /// Lấy các subscription còn hiệu lực của người dùng.
    /// Luồng chỉ giữ trạng thái active và chưa hết hạn để tính entitlement chính xác.
    /// </summary>
    public Task<List<UserSubscription>> GetActiveSubscriptionsAsync(Guid userId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        // Lọc theo trạng thái và hạn dùng để tránh cấp quyền từ subscription đã hết hiệu lực.
        return _context.UserSubscriptions
            .Include(s => s.Plan)
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active && s.EndDate > now)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Lấy batch subscription active đã hết hạn để tiến trình nền xử lý chuyển trạng thái.
    /// Luồng giới hạn 50 bản ghi giúp job chạy đều và giảm lock dài.
    /// </summary>
    public Task<List<UserSubscription>> GetExpiredSubscriptionsToProcessAsync(DateTime cutoff, CancellationToken ct)
    {
        return _context.UserSubscriptions
            .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= cutoff)
            .OrderBy(s => s.EndDate)
            .Take(50)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Thêm subscription mới vào context.
    /// Luồng tách add/commit để phối hợp với các bước tạo bucket entitlement trong cùng transaction.
    /// </summary>
    public Task AddSubscriptionAsync(UserSubscription subscription, CancellationToken ct)
    {
        // Đánh dấu entity ở trạng thái Added để commit cùng các thay đổi liên quan.
        _context.UserSubscriptions.Add(subscription);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Cập nhật subscription hiện có vào context.
    /// Luồng này dùng khi gia hạn hoặc chuyển trạng thái subscription.
    /// </summary>
    public Task UpdateSubscriptionAsync(UserSubscription subscription, CancellationToken ct)
    {
        // Đưa entity về trạng thái Modified để lưu thay đổi ở lần SaveChanges kế tiếp.
        _context.UserSubscriptions.Update(subscription);
        return Task.CompletedTask;
    }
}
