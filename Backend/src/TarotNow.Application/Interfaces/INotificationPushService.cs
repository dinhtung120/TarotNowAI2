/*
 * ===================================================================
 * FILE: INotificationPushService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Interface định nghĩa contract cho việc PUSH thông báo real-time
 *   xuống client qua SignalR (hoặc bất kỳ transport nào khác).
 *
 * TẠI SAO CẦN INTERFACE RIÊNG?
 *   - Tuân thủ Clean Architecture: tầng Application không biết về SignalR.
 *   - Tầng Infrastructure implement bằng IHubContext<PresenceHub>.
 *   - Dễ mock trong unit test, dễ swap transport trong tương lai.
 *
 * TẠI SAO DÙNG NotificationCreateDto THAY VÌ NotificationDto?
 *   - CreateAsync (MongoDB) không trả về document đã lưu (bao gồm Id).
 *   - Push real-time chỉ cần signal để FE invalidate cache + refetch,
 *     KHÔNG cần Id chính xác → NotificationCreateDto là đủ.
 * ===================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Service đẩy thông báo real-time xuống client.
/// Được gọi sau khi thông báo đã được lưu vào MongoDB.
/// </summary>
public interface INotificationPushService
{
    /// <summary>
    /// Push event "notification.new" xuống client qua kênh real-time (SignalR).
    /// FE nhận event → invalidate React Query cache → UI tự refetch.
    /// </summary>
    /// <param name="notification">DTO chứa thông tin thông báo vừa được tạo.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    Task PushNewNotificationAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Push custom event xuống client (e.g., gamification events).
    /// </summary>
    Task SendEventAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default);
}
