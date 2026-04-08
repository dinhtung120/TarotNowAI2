using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    /// <summary>
    /// Lấy các mapping rule đang bật cho entitlement nguồn.
    /// Luồng này tách rule khả dụng để tầng nghiệp vụ map quyền lợi đúng cấu hình runtime.
    /// </summary>
    public Task<List<EntitlementMappingRule>> GetEnabledMappingRulesAsync(string sourceKey, CancellationToken ct)
    {
        return _context.EntitlementMappingRules
            .Where(m => m.SourceKey == sourceKey && m.IsEnabled)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Thêm log consume entitlement để phục vụ đối soát và idempotency.
    /// Luồng chỉ thêm vào context, việc commit được điều phối ở tầng transaction gọi ngoài.
    /// </summary>
    public Task AddConsumeLogAsync(EntitlementConsume consume, CancellationToken ct)
    {
        // Ghi nhận state consume trước khi SaveChanges để không mất dấu vết giao dịch entitlement.
        _context.EntitlementConsumes.Add(consume);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Kiểm tra consume log đã tồn tại theo idempotency key hay chưa.
    /// Luồng này chặn xử lý lặp khi client retry cùng yêu cầu.
    /// </summary>
    public Task<bool> ConsumeLogExistsAsync(string idempotencyKey, CancellationToken ct)
    {
        return _context.EntitlementConsumes.AnyAsync(c => c.IdempotencyKey == idempotencyKey, ct);
    }

    /// <summary>
    /// Lưu toàn bộ thay đổi entitlement trong unit-of-work hiện tại.
    /// Luồng commit tập trung giúp các thao tác add log và cập nhật bucket đi cùng một nhịp.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
