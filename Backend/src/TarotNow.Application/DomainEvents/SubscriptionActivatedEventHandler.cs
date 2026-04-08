using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents;

// Handler dọn cache entitlement khi subscription được kích hoạt.
public class SubscriptionActivatedEventHandler : INotificationHandler<SubscriptionActivatedNotification>
{
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Khởi tạo handler subscription activated.
    /// Luồng xử lý: nhận cache service để làm mới dữ liệu entitlement sau khi kích hoạt gói.
    /// </summary>
    public SubscriptionActivatedEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Xử lý notification kích hoạt subscription bằng cách xóa cache entitlement.
    /// Luồng xử lý: lấy user id từ domain event, dựng cache key rồi remove key tương ứng.
    /// </summary>
    public async Task Handle(SubscriptionActivatedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // Khi trạng thái subscription đổi, bắt buộc xóa cache để client đọc quyền lợi mới nhất.
        var cacheKey = $"entitlement_balance:{domainEvent.UserId}";
        await _cacheService.RemoveAsync(cacheKey);
    }
}
