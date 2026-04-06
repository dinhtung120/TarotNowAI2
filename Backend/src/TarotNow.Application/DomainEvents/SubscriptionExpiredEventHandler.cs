/*
 * ===================================================================
 * FILE: SubscriptionExpiredEventHandler.cs
 * NAMESPACE: TarotNow.Application.DomainEvents
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gắn Nghe Tiếng Báo Thức Hỏng Gói Của Job Nền.
 *   Xóa Bức Tường Cache UI Để App Tắt Không Tím Nút Hiện (Dẹp Nút Dịch Vụ Đặc Quyền Free Khỏi Mắt User Ngay).
 * ===================================================================
 */

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents;

public class SubscriptionExpiredEventHandler : INotificationHandler<SubscriptionExpiredNotification>
{
    private readonly ICacheService _cacheService;

    public SubscriptionExpiredEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(SubscriptionExpiredNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        // Nổ Quỹ Cache Số Dư Đặc Sủng Liên Quan Client Màn Hình Đọc Do Sập Ngày.
        var cacheKey = $"entitlement_balance:{ev.UserId}";
        await _cacheService.RemoveAsync(cacheKey);
    }
}
