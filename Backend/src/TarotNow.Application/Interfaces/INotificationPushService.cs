

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract đẩy thông báo realtime để đồng bộ trạng thái notification trên client.
public interface INotificationPushService
{
    /// <summary>
    /// Đẩy một thông báo mới tới người dùng ngay sau khi bản ghi được tạo.
    /// Luồng xử lý: nhận DTO notification đã chuẩn hóa và phát sự kiện tới kênh realtime đích.
    /// </summary>
    Task PushNewNotificationAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gửi sự kiện tùy biến cho người dùng để hỗ trợ các luồng cập nhật giao diện khác nhau.
    /// Luồng xử lý: publish eventName cùng payload tới userId tương ứng.
    /// </summary>
    Task SendEventAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default);
}
