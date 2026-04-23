using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Theo dõi trạng thái online theo connection + heartbeat để phục vụ hiển thị hiện diện realtime.
public class InMemoryUserPresenceTracker : IUserPresenceTracker
{
    // Mỗi user có nhiều connection đồng thời (multi-tab/multi-device), lưu dạng concurrent để an toàn luồng.
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _connectionsByUser = new(StringComparer.OrdinalIgnoreCase);
    // Lưu mốc hoạt động gần nhất để fallback online khi kết nối realtime vừa gián đoạn ngắn.
    private readonly ConcurrentDictionary<string, DateTime> _lastActivity = new(StringComparer.OrdinalIgnoreCase);
    private readonly ISystemConfigSettings _systemConfigSettings;

    public InMemoryUserPresenceTracker(ISystemConfigSettings systemConfigSettings)
    {
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Đánh dấu một kết nối mới của user.
    /// Luồng xử lý: validate input, thêm connection vào tập của user, rồi cập nhật heartbeat.
    /// </summary>
    public void MarkConnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            // Edge case: dữ liệu kết nối thiếu thì bỏ qua để tránh tạo state rác.
            return;
        }

        var userConnections = _connectionsByUser.GetOrAdd(
            userId,
            _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        // Cập nhật trạng thái kết nối hiện tại của user; ghi đè an toàn nếu connection đã tồn tại.
        userConnections[connectionId] = 1;

        // Sau khi có kết nối hợp lệ, ghi heartbeat để đồng bộ logic timeout.
        RecordHeartbeat(userId);
    }

    /// <summary>
    /// Đánh dấu ngắt kết nối cho một connection của user.
    /// Luồng xử lý: validate input, remove connection, dọn user khỏi map nếu hết connection, rồi cập nhật heartbeat.
    /// </summary>
    public void MarkDisconnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            // Edge case: request ngắt kết nối không đủ dữ liệu thì không thay đổi state.
            return;
        }

        if (_connectionsByUser.TryGetValue(userId, out var userConnections))
        {
            userConnections.TryRemove(connectionId, out _);
            if (userConnections.IsEmpty)
            {
                // Khi user không còn connection hoạt động, xóa hẳn để giảm bộ nhớ và tránh false-positive online.
                _connectionsByUser.TryRemove(userId, out _);
            }
        }

        // Vẫn ghi heartbeat để phản ánh thời điểm cuối cùng user còn tương tác hệ thống.
        RecordHeartbeat(userId);
    }

    /// <summary>
    /// Kiểm tra user có đang online theo kết nối realtime hoặc heartbeat gần nhất hay không.
    /// Luồng xử lý: ưu tiên trạng thái connection sống, fallback theo cửa sổ thời gian hoạt động gần nhất.
    /// </summary>
    public bool IsOnline(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            // Edge case: thiếu user id thì mặc định offline để tránh cấp quyền nhầm.
            return false;
        }

        if (_connectionsByUser.TryGetValue(userId, out var userConnections) && !userConnections.IsEmpty)
        {
            // Có ít nhất một kết nối đang mở thì xác định online ngay.
            return true;
        }

        if (_lastActivity.TryGetValue(userId, out var lastActivityTime))
        {
            // Fallback nghiệp vụ: coi là online trong 15 phút gần nhất để tránh nhấp nháy trạng thái.
            var onlineWindowMinutes = Math.Clamp(_systemConfigSettings.PresenceTimeoutMinutes, 1, 240);
            return (DateTime.UtcNow - lastActivityTime).TotalMinutes <= onlineWindowMinutes;
        }

        // Không có connection và không có heartbeat thì xem là offline.
        return false;
    }

    /// <summary>
    /// Ghi nhận thời điểm hoạt động gần nhất của user.
    /// Luồng xử lý: chỉ cập nhật khi user id hợp lệ để tránh key rỗng.
    /// </summary>
    public void RecordHeartbeat(string userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Ghi đè thời điểm mới nhất để các luồng timeout/online cùng đọc một nguồn dữ liệu.
            _lastActivity[userId] = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Trả về thời điểm hoạt động cuối cùng của user nếu có.
    /// Luồng xử lý: validate user id rồi tra cứu trong bảng heartbeat.
    /// </summary>
    public DateTime? GetLastActivity(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            // Edge case: user id không hợp lệ thì không thể có mốc hoạt động.
            return null;
        }

        return _lastActivity.TryGetValue(userId, out var lastActivityTime) ? lastActivityTime : null;
    }

    /// <summary>
    /// Lấy danh sách user đã quá hạn hoạt động và không còn connection active.
    /// Luồng xử lý: tính mốc cutoff theo timeout, lọc bằng LINQ theo heartbeat + trạng thái connection.
    /// </summary>
    public IReadOnlyList<string> GetTimedOutUsers(TimeSpan timeout)
    {
        var cutoffTime = DateTime.UtcNow - timeout;

        // LINQ kết hợp hai điều kiện nhằm tránh timeout nhầm user vẫn đang có connection sống.
        return _lastActivity
            .Where(kvp => kvp.Value <= cutoffTime &&
                          (!_connectionsByUser.TryGetValue(kvp.Key, out var userConnections) || userConnections.IsEmpty))
            .Select(kvp => kvp.Key)
            .ToList();
    }

    /// <summary>
    /// Xóa toàn bộ trạng thái hiện diện của một user khỏi bộ nhớ.
    /// Luồng xử lý: validate user id rồi remove cả map connection và heartbeat.
    /// </summary>
    public void RemoveUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            // Edge case: key rỗng thì không thực hiện để tránh thao tác không xác định.
            return;
        }

        // Dọn đồng bộ hai nguồn state để tránh lệch dữ liệu online/offline.
        _connectionsByUser.TryRemove(userId, out _);
        _lastActivity.TryRemove(userId, out _);
    }
}
