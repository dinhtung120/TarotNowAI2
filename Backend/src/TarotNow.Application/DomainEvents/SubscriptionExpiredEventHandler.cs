using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents;

// Handler dọn cache entitlement khi subscription hết hạn.
public class SubscriptionExpiredEventHandler : INotificationHandler<SubscriptionExpiredNotification>
{
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Khởi tạo handler subscription expired.
    /// Luồng xử lý: nhận cache service để loại bỏ dữ liệu quyền lợi cũ sau khi gói hết hạn.
    /// </summary>
    public SubscriptionExpiredEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Xử lý notification hết hạn subscription bằng cách xóa cache entitlement.
    /// Luồng xử lý: lấy user id từ domain event, dựng cache key và remove key khỏi cache.
    /// </summary>
    public async Task Handle(SubscriptionExpiredNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // Dọn cache ngay để các request tiếp theo không dùng quyền lợi đã hết hạn.
        var cacheKey = $"entitlement_balance:{domainEvent.UserId}";
        await _cacheService.RemoveAsync(cacheKey);
    }
}
