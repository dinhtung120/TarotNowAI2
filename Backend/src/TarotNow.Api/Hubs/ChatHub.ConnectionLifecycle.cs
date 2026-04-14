namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Xử lý khi kết nối chat hub được thiết lập.
    /// Luồng xử lý: thêm connection vào user-group, ghi log kết nối, tiếp tục pipeline SignalR.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId) == false)
        {
            try
            {
                // Join user-group để server có thể push notification theo user id.
                await Groups.AddToGroupAsync(Context.ConnectionId, UserGroup(userId));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "[ChatHub] Failed to add connection to user group. UserId={UserId}, ConnectionId={ConnectionId}",
                    userId,
                    Context.ConnectionId);
            }
        }

        _logger.LogDebug(
            "[ChatHub] User {UserId} connected. ConnectionId: {ConnectionId}",
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Xử lý khi kết nối chat hub bị ngắt.
    /// Luồng xử lý: gỡ connection khỏi user-group, ghi log nguyên nhân ngắt, tiếp tục pipeline SignalR.
    /// </summary>
    /// <param name="exception">Exception ngắt kết nối nếu có.</param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId) == false)
        {
            try
            {
                // Gỡ khỏi user-group để tránh broadcast thừa tới connection đã đóng.
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, UserGroup(userId));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "[ChatHub] Failed to remove connection from user group. UserId={UserId}, ConnectionId={ConnectionId}",
                    userId,
                    Context.ConnectionId);
            }
        }

        _logger.LogDebug(
            "[ChatHub] User {UserId} disconnected. Reason: {Reason}",
            userId,
            exception?.Message ?? "normal");

        await base.OnDisconnectedAsync(exception);
    }
}
