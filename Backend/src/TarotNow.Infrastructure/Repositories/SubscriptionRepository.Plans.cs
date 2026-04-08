using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    /// <summary>
    /// Lấy thông tin gói subscription theo định danh.
    /// Luồng này hỗ trợ validate đầu vào trước khi tạo subscription cho người dùng.
    /// </summary>
    public Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId, CancellationToken ct)
    {
        return _context.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == planId, ct);
    }

    /// <summary>
    /// Lấy danh sách gói đang hoạt động để hiển thị hoặc bán cho người dùng.
    /// Luồng sắp xếp theo thứ tự hiển thị giúp API trả dữ liệu ổn định.
    /// </summary>
    public Task<List<SubscriptionPlan>> GetActivePlansAsync(CancellationToken ct)
    {
        return _context.SubscriptionPlans
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Thêm gói subscription mới vào DbContext.
    /// Luồng tách bước add và commit để tái sử dụng trong nghiệp vụ admin.
    /// </summary>
    public Task AddPlanAsync(SubscriptionPlan plan, CancellationToken ct)
    {
        // Đưa plan vào tracked state để SaveChanges xử lý đồng bộ với các thay đổi liên quan.
        _context.SubscriptionPlans.Add(plan);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Đánh dấu gói subscription đã thay đổi để cập nhật.
    /// Luồng này đảm bảo chỉnh sửa metadata gói được lưu ở lần commit kế tiếp.
    /// </summary>
    public Task UpdatePlanAsync(SubscriptionPlan plan, CancellationToken ct)
    {
        // Cập nhật state entity nhằm đồng bộ thay đổi cấu hình plan vào database.
        _context.SubscriptionPlans.Update(plan);
        return Task.CompletedTask;
    }
}
