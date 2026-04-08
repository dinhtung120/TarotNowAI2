

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract dữ liệu subscription để quản lý gói, đăng ký, entitlement bucket và log tiêu thụ.
public interface ISubscriptionRepository
{
    /// <summary>
    /// Lấy thông tin gói subscription theo id để xử lý mua gói hoặc hiển thị chi tiết.
    /// Luồng xử lý: truy vấn planId và trả null khi gói không tồn tại.
    /// </summary>
    Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId, CancellationToken ct);

    /// <summary>
    /// Lấy các gói đang hoạt động để hiển thị danh sách mua gói cho người dùng.
    /// Luồng xử lý: lọc theo trạng thái active và trả danh sách plan khả dụng.
    /// </summary>
    Task<List<SubscriptionPlan>> GetActivePlansAsync(CancellationToken ct);

    /// <summary>
    /// Tạo gói subscription mới khi có cấu hình sản phẩm mới.
    /// Luồng xử lý: persist entity plan vào kho dữ liệu.
    /// </summary>
    Task AddPlanAsync(SubscriptionPlan plan, CancellationToken ct);

    /// <summary>
    /// Cập nhật gói subscription để điều chỉnh giá hoặc quyền lợi.
    /// Luồng xử lý: ghi đè dữ liệu plan theo định danh gói tương ứng.
    /// </summary>
    Task UpdatePlanAsync(SubscriptionPlan plan, CancellationToken ct);

    /// <summary>
    /// Lấy subscription theo idempotency key để chống tạo trùng khi retry thanh toán.
    /// Luồng xử lý: truy vấn theo idempotencyKey và trả subscription đã tồn tại nếu có.
    /// </summary>
    Task<UserSubscription?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct);

    /// <summary>
    /// Lấy các subscription đang active của người dùng để xác định quyền lợi hiện hành.
    /// Luồng xử lý: lọc theo userId và trạng thái hiệu lực hiện tại.
    /// </summary>
    Task<List<UserSubscription>> GetActiveSubscriptionsAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Lấy các subscription đã hết hạn trước cutoff để chạy luồng xử lý hậu kỳ.
    /// Luồng xử lý: lọc theo ngày hết hạn và trạng thái cần xử lý.
    /// </summary>
    Task<List<UserSubscription>> GetExpiredSubscriptionsToProcessAsync(DateTime cutoff, CancellationToken ct);

    /// <summary>
    /// Tạo bản ghi subscription mới sau khi người dùng mua gói thành công.
    /// Luồng xử lý: persist entity subscription vào dữ liệu bền vững.
    /// </summary>
    Task AddSubscriptionAsync(UserSubscription subscription, CancellationToken ct);

    /// <summary>
    /// Cập nhật subscription khi gia hạn, hủy hoặc thay đổi trạng thái.
    /// Luồng xử lý: ghi các thay đổi của subscription theo bản ghi hiện có.
    /// </summary>
    Task UpdateSubscriptionAsync(UserSubscription subscription, CancellationToken ct);

    /// <summary>
    /// Thêm các entitlement bucket mới để cấp quota theo subscription vừa kích hoạt.
    /// Luồng xử lý: chèn danh sách bucket theo batch trong cùng luồng xử lý.
    /// </summary>
    Task AddBucketsAsync(List<SubscriptionEntitlementBucket> buckets, CancellationToken ct);

    /// <summary>
    /// Lấy bucket đủ điều kiện tiêu thụ entitlement trong ngày nghiệp vụ hiện tại.
    /// Luồng xử lý: lọc theo userId/entitlementKey/businessDate và trả danh sách bucket khả dụng.
    /// </summary>
    Task<List<SubscriptionEntitlementBucket>> GetBucketsForConsumeAsync(
        Guid userId,
        string entitlementKey,
        DateOnly businessDate,
        CancellationToken ct);

    /// <summary>
    /// Lấy tổng hợp số dư entitlement theo ngày để hiển thị dashboard quyền lợi.
    /// Luồng xử lý: gom dữ liệu bucket theo userId/businessDate và trả danh sách balance.
    /// </summary>
    Task<List<EntitlementBalanceDto>> GetBalanceSummaryAsync(
        Guid userId,
        DateOnly businessDate,
        CancellationToken ct);

    /// <summary>
    /// Lấy toàn bộ bucket cần reset khi sang ngày nghiệp vụ mới.
    /// Luồng xử lý: lọc bucket theo oldBusinessDate và trả danh sách cần xử lý batch.
    /// </summary>
    Task<List<SubscriptionEntitlementBucket>> GetAllBucketsForResetAsync(
        DateOnly oldBusinessDate,
        CancellationToken ct);

    /// <summary>
    /// Lấy mapping rule đang bật theo source key để ánh xạ entitlement đúng ngữ cảnh.
    /// Luồng xử lý: lọc rule enabled theo sourceKey và trả danh sách áp dụng.
    /// </summary>
    Task<List<EntitlementMappingRule>> GetEnabledMappingRulesAsync(
        string sourceKey,
        CancellationToken ct);

    /// <summary>
    /// Ghi log tiêu thụ entitlement để phục vụ đối soát và idempotency.
    /// Luồng xử lý: persist bản ghi consume vào lịch sử tiêu thụ.
    /// </summary>
    Task AddConsumeLogAsync(EntitlementConsume consume, CancellationToken ct);

    /// <summary>
    /// Kiểm tra log tiêu thụ đã tồn tại theo idempotency key để ngăn xử lý lặp.
    /// Luồng xử lý: truy vấn theo key và trả true nếu đã có consume log trước đó.
    /// </summary>
    Task<bool> ConsumeLogExistsAsync(string idempotencyKey, CancellationToken ct);

    /// <summary>
    /// Lưu toàn bộ thay đổi pending của ngữ cảnh subscription trong cùng unit of work.
    /// Luồng xử lý: commit transaction hiện tại để đảm bảo dữ liệu liên quan được ghi đồng bộ.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct);
}
