using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

public partial class EntitlementService
{
    /// <summary>
    /// Tiêu thụ một entitlement của người dùng theo yêu cầu nghiệp vụ.
    /// Luồng chạy trong transaction, xử lý idempotency, cập nhật bucket, ghi log và xóa cache số dư.
    /// </summary>
    public async Task<EntitlementConsumeResult> ConsumeAsync(EntitlementConsumeRequest request, CancellationToken ct)
    {
        var outcome = new EntitlementConsumeResult(false, "Unknown error");
        await _transactionCoordinator.ExecuteAsync(
            async cancellation => outcome = await ConsumeCoreAsync(request, cancellation),
            ct);

        if (outcome.Success)
        {
            // Consume thành công thì xóa cache để lần đọc số dư sau luôn phản ánh dữ liệu mới.
            await _cacheService.RemoveAsync(BuildBalanceCacheKey(request.UserId));
        }

        return outcome;
    }

    /// <summary>
    /// Thực thi phần cốt lõi của consume entitlement trong transaction.
    /// Luồng ưu tiên kiểm tra idempotency trước, sau đó chọn bucket hợp lệ và ghi nhận consume.
    /// </summary>
    private async Task<EntitlementConsumeResult> ConsumeCoreAsync(EntitlementConsumeRequest request, CancellationToken ct)
    {
        if (await _repository.ConsumeLogExistsAsync(request.IdempotencyKey, ct))
        {
            // Đã có log cùng idempotency key thì trả thành công giả lập để chống xử lý lặp.
            _logger.LogInformation("Consume Idempotency Key {Key} already exists.", request.IdempotencyKey);
            return new EntitlementConsumeResult(true, "Already consumed (idempotent)");
        }

        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        var buckets = await _repository.GetBucketsForConsumeAsync(request.UserId, request.EntitlementKey, todayUtc, ct);
        var targetBucket = buckets.FirstOrDefault(b => b.CanConsume(todayUtc));
        if (targetBucket == null)
        {
            // Không còn bucket khả dụng thì trả kết quả hết quota để caller xử lý đúng thông điệp.
            return await BuildNoQuotaResultAsync(request.EntitlementKey, ct);
        }

        await ConsumeBucketAsync(request, targetBucket, todayUtc, ct);
        return new EntitlementConsumeResult(true, "Consumed successfully");
    }

    /// <summary>
    /// Tiêu thụ quota trên bucket đã chọn và ghi nhận log consume.
    /// Luồng cập nhật state bucket, lưu consume log, commit rồi phát domain event hậu xử lý.
    /// </summary>
    private async Task ConsumeBucketAsync(
        EntitlementConsumeRequest request,
        SubscriptionEntitlementBucket bucket,
        DateOnly businessDate,
        CancellationToken ct)
    {
        // Trừ quota trên bucket trước để trạng thái entitlement phản ánh ngay trong transaction.
        bucket.Consume(businessDate);
        var log = new EntitlementConsume(
            userId: request.UserId,
            bucketId: bucket.Id,
            entitlementKey: request.EntitlementKey,
            referenceSource: request.ReferenceSource,
            referenceId: request.ReferenceId,
            idempotencyKey: request.IdempotencyKey);

        // Ghi consume log làm bằng chứng nghiệp vụ và nền tảng cho kiểm tra idempotency.
        await _repository.AddConsumeLogAsync(log, ct);
        // Commit để cố định thay đổi bucket + log trước khi phát event.
        await _repository.SaveChangesAsync(ct);
        // Phát event sau commit để các subscriber đọc được trạng thái mới nhất.
        await _domainEventPublisher.PublishAsync(
            new EntitlementConsumedDomainEvent(request.UserId, request.EntitlementKey, bucket.Id),
            ct);
    }

    /// <summary>
    /// Tạo kết quả thất bại khi không còn quota entitlement khả dụng.
    /// Luồng vẫn đọc mapping rule để log cảnh báo vận hành khi policy map chéo đang bị vô hiệu.
    /// </summary>
    private async Task<EntitlementConsumeResult> BuildNoQuotaResultAsync(string entitlementKey, CancellationToken ct)
    {
        var mapRules = await _repository.GetEnabledMappingRulesAsync(entitlementKey, ct);
        if (mapRules.Any())
        {
            // Có rule map chéo nhưng policy hiện tại không cho dùng, cần log để hỗ trợ điều tra.
            _logger.LogInformation("Cross mapping is configured but currently disabled by business policy.");
        }

        return new EntitlementConsumeResult(false, "No active quota available for this entitlement");
    }

    /// <summary>
    /// Tạo cache key số dư entitlement theo user.
    /// Luồng tách helper để đồng bộ key format giữa thao tác đọc và xóa cache.
    /// </summary>
    private static string BuildBalanceCacheKey(Guid userId) => $"entitlement_balance:{userId}";
}
