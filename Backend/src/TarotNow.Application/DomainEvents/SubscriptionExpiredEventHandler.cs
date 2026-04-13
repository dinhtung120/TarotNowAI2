using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents;

// Handler dọn cache entitlement khi subscription hết hạn.
public class SubscriptionExpiredEventHandler
    : IdempotentDomainEventNotificationHandler<SubscriptionExpiredDomainEvent>
{
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Khởi tạo handler subscription expired.
    /// Luồng xử lý: nhận cache service để loại bỏ dữ liệu quyền lợi cũ sau khi gói hết hạn.
    /// </summary>
    public SubscriptionExpiredEventHandler(
        ICacheService cacheService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Xử lý notification hết hạn subscription bằng cách xóa cache entitlement.
    /// Luồng xử lý: lấy user id từ domain event, dựng cache key và remove key khỏi cache.
    /// </summary>
    protected override async Task HandleDomainEventAsync(
        SubscriptionExpiredDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // Dọn cache ngay để các request tiếp theo không dùng quyền lợi đã hết hạn.
        var cacheKey = $"entitlement_balance:{domainEvent.UserId}";
        await _cacheService.RemoveAsync(cacheKey);
    }
}
