using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    // Khoảng đệm giữ session ngắn để tránh cleanup nhầm khi client reconnect nhanh.
    private static readonly TimeSpan DisconnectGracePeriod = TimeSpan.FromSeconds(12);

    // Bản đồ user -> tập connection id đang hoạt động.
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> UserConnections =
        new(StringComparer.Ordinal);

    // Cache quyền truy cập conversation theo user để giảm truy vấn lặp.
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> UserConversationAccess =
        new(StringComparer.Ordinal);

    // Theo dõi job cleanup disconnect chờ thực thi.
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> PendingDisconnectCleanup =
        new(StringComparer.Ordinal);

    /// <summary>
    /// Đăng ký connection mới cho user.
    /// Luồng xử lý: hủy cleanup treo, tạo map connection nếu chưa có, thêm connection hiện tại.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <param name="connectionId">Connection id của SignalR.</param>
    private static void RegisterConnection(string userId, string connectionId)
    {
        // Reconnect thành công thì phải hủy cleanup cũ để tránh xóa trạng thái nhầm.
        CancelPendingDisconnectCleanup(userId);
        var connectionMap = UserConnections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        connectionMap[connectionId] = 0;
    }

    /// <summary>
    /// Gỡ connection khỏi bảng theo dõi của user.
    /// Luồng xử lý: xóa connection cụ thể, nếu user không còn kết nối thì dọn toàn bộ cache phụ trợ.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <param name="connectionId">Connection id cần gỡ.</param>
    private static void UnregisterConnection(string userId, string connectionId)
    {
        if (!UserConnections.TryGetValue(userId, out var connectionMap))
        {
            // Không có map tương ứng thì xem như đã dọn xong trước đó.
            return;
        }

        connectionMap.TryRemove(connectionId, out _);
        if (connectionMap.IsEmpty)
        {
            // Khi user không còn connection nào thì dọn luôn cache truy cập conversation liên quan.
            UserConnections.TryRemove(userId, out _);
            UserConversationAccess.TryRemove(userId, out _);
        }
    }

    /// <summary>
    /// Kiểm tra user còn connection hoạt động nào hay không.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <returns><c>true</c> nếu còn ít nhất một connection.</returns>
    private static bool HasAnyConnection(string userId)
    {
        return UserConnections.TryGetValue(userId, out var connectionMap) && !connectionMap.IsEmpty;
    }

    /// <summary>
    /// Ghi nhớ quyền truy cập conversation đơn lẻ cho user.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <param name="conversationId">Conversation id đã xác minh quyền.</param>
    private static void RememberConversationAccess(string userId, string conversationId)
    {
        var accessMap = UserConversationAccess.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        accessMap[conversationId] = 0;
    }

    /// <summary>
    /// Ghi nhớ quyền truy cập nhiều conversation cho user.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <param name="conversationIds">Danh sách conversation id cần cache quyền truy cập.</param>
    private static void RememberConversationAccess(string userId, IEnumerable<string> conversationIds)
    {
        var accessMap = UserConversationAccess.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        foreach (var conversationId in conversationIds.Where(id => !string.IsNullOrWhiteSpace(id)))
        {
            // Bỏ qua id rỗng để tránh cache key không hợp lệ.
            accessMap[conversationId] = 0;
        }
    }

    /// <summary>
    /// Kiểm tra nhanh quyền truy cập conversation trong cache memory.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <param name="conversationId">Conversation id cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đã có quyền trong cache.</returns>
    private static bool HasConversationAccessCached(string userId, string conversationId)
    {
        return UserConversationAccess.TryGetValue(userId, out var accessMap) && accessMap.ContainsKey(conversationId);
    }

    /// <summary>
    /// Trì hoãn cleanup để hấp thụ ngắt kết nối tạm thời.
    /// Luồng xử lý: chờ grace period, kiểm tra lại connection còn hay không, rồi mới cleanup nếu cần.
    /// </summary>
    /// <param name="userId">User id cần xử lý cleanup.</param>
    private async Task DelayCleanupForTransientDisconnectAsync(string userId)
    {
        var cleanupCts = ReplacePendingDisconnectCleanup(userId);

        try
        {
            await Task.Delay(DisconnectGracePeriod, cleanupCts.Token);
        }
        catch (OperationCanceledException)
        {
            // Bị hủy do reconnect hoặc cleanup mới thay thế thì dừng nhánh hiện tại.
            return;
        }
        finally
        {
            if (PendingDisconnectCleanup.TryGetValue(userId, out var current)
                && ReferenceEquals(current, cleanupCts))
            {
                PendingDisconnectCleanup.TryRemove(userId, out _);
                cleanupCts.Dispose();
            }
        }

        if (HasAnyConnection(userId))
        {
            // User đã reconnect trong thời gian grace thì không cleanup trạng thái cuộc gọi.
            return;
        }

        await HandleDisconnectedUserCleanupAsync(userId);
    }

    /// <summary>
    /// Thay thế job cleanup pending hiện tại bằng token mới.
    /// </summary>
    /// <param name="userId">User id cần thay thế cleanup token.</param>
    /// <returns>CancellationTokenSource mới được đăng ký.</returns>
    private static CancellationTokenSource ReplacePendingDisconnectCleanup(string userId)
    {
        CancelPendingDisconnectCleanup(userId);
        var cts = new CancellationTokenSource();
        PendingDisconnectCleanup[userId] = cts;
        return cts;
    }

    /// <summary>
    /// Hủy và dọn cleanup pending của user nếu tồn tại.
    /// </summary>
    /// <param name="userId">User id cần hủy cleanup pending.</param>
    private static void CancelPendingDisconnectCleanup(string userId)
    {
        if (PendingDisconnectCleanup.TryRemove(userId, out var existing))
        {
            existing.Cancel();
            existing.Dispose();
        }
    }
}
