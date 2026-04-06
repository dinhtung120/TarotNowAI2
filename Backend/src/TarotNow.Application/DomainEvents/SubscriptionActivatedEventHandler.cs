/*
 * ===================================================================
 * FILE: SubscriptionActivatedEventHandler.cs
 * NAMESPACE: TarotNow.Application.DomainEvents
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lắng nghe khi Khách Mua Hàng Xong Chốt Vở Kịch Bọn Domain Bắn Ra.
 *   Xử lý công việc dọn dẹp Hậu Cần: Xóa Cache Cũ Gấp Nhanh Cho User Nhìn Thấy Chữ Active Gói Bóng Bẩy Liên Ngay + Gửi Notification Keng Keng Điện Thoại.
 * ===================================================================
 */

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents;

public class SubscriptionActivatedEventHandler : INotificationHandler<SubscriptionActivatedNotification>
{
    private readonly ICacheService _cacheService;
    // Tạm chưa code IUserNotificationService để Push vì Focus Core Đầu Tiên Lỗ Hổng Tài Chính.

    public SubscriptionActivatedEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(SubscriptionActivatedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        // Phá Vỡ Quỹ Căn Cơ Redis Tại Chìa Khóa "Sổ Dư Rút Về Này Đã Lỗi Thời Châm Kịp Nạp Thêm Gói Rồi"
        var cacheKey = $"entitlement_balance:{ev.UserId}";
        await _cacheService.RemoveAsync(cacheKey);

        // TODO: Gắn Thêm INotificationPushService Vào Để Bắn Push Cho Real-Time Keng Chúc Mừng. 
        // Phía Lõi Focus: Đã Giữ Được Nguyên Trạng Database.
    }
}
