using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents;

// Handler dọn cache entitlement khi đã tiêu thụ quyền lợi subscription.
public class EntitlementConsumedEventHandler : INotificationHandler<EntitlementConsumedNotification>
{
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Khởi tạo handler tiêu thụ entitlement.
    /// Luồng xử lý: nhận cache service để xóa key balance entitlement liên quan người dùng.
    /// </summary>
    public EntitlementConsumedEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Xử lý sự kiện entitlement đã bị trừ và xóa cache balance để buộc đọc mới từ nguồn dữ liệu chuẩn.
    /// Luồng xử lý: lấy domain event, dựng cache key theo user id, gọi remove cache bất đồng bộ.
    /// </summary>
    public async Task Handle(EntitlementConsumedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // Đổi state cache ngay sau khi entitlement thay đổi để tránh client đọc số dư cũ.
        var cacheKey = $"entitlement_balance:{domainEvent.UserId}";
        await _cacheService.RemoveAsync(cacheKey);
    }
}
