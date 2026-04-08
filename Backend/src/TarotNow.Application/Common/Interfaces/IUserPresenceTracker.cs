using System;
using System.Collections.Generic;

namespace TarotNow.Application.Common.Interfaces;

// Hợp đồng theo dõi trạng thái hiện diện người dùng dùng chung cho realtime và background cleanup.
public interface IUserPresenceTracker
{
    /// <summary>
    /// Đánh dấu user vừa mở thêm một kết nối realtime.
    /// Luồng xử lý: nhận user id + connection id để cập nhật trạng thái online.
    /// </summary>
    void MarkConnected(string userId, string connectionId);

    /// <summary>
    /// Đánh dấu user đóng một kết nối realtime.
    /// Luồng xử lý: remove connection tương ứng để phục vụ tính online/offline chính xác.
    /// </summary>
    void MarkDisconnected(string userId, string connectionId);

    /// <summary>
    /// Kiểm tra user có đang online hay không.
    /// Luồng xử lý: dựa trên connection active hoặc heartbeat gần nhất theo triển khai cụ thể.
    /// </summary>
    bool IsOnline(string userId);

    /// <summary>
    /// Ghi nhận heartbeat hoạt động của user.
    /// Luồng xử lý: cập nhật mốc thời gian hoạt động cuối cùng cho user.
    /// </summary>
    void RecordHeartbeat(string userId);

    /// <summary>
    /// Lấy thời điểm hoạt động gần nhất của user.
    /// Luồng xử lý: trả null khi chưa có dữ liệu heartbeat.
    /// </summary>
    DateTime? GetLastActivity(string userId);

    /// <summary>
    /// Trả danh sách user đã quá hạn hoạt động theo timeout chỉ định.
    /// Luồng xử lý: áp dụng timeout để lọc user không còn active.
    /// </summary>
    IReadOnlyList<string> GetTimedOutUsers(TimeSpan timeout);

    /// <summary>
    /// Xóa toàn bộ trạng thái hiện diện của một user.
    /// Luồng xử lý: dọn dữ liệu connection và heartbeat của user khỏi tracker.
    /// </summary>
    void RemoveUser(string userId);
}
