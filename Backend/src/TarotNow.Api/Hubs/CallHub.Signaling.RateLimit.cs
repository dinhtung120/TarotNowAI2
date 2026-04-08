namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Thử lấy quyền thao tác signaling theo cửa sổ rate-limit.
    /// Luồng xử lý: kiểm tra key trong cache, trả lỗi realtime nếu vượt ngưỡng.
    /// </summary>
    /// <param name="operation">Tên thao tác signaling.</param>
    /// <param name="userId">User id thực hiện thao tác.</param>
    /// <param name="window">Cửa sổ thời gian giới hạn.</param>
    /// <param name="rejectMessage">Thông điệp trả về khi bị rate-limit.</param>
    /// <returns><c>true</c> nếu được phép tiếp tục; ngược lại <c>false</c>.</returns>
    private async Task<bool> TryAcquireSignalRateLimitAsync(string operation, Guid userId, TimeSpan window, string rejectMessage)
    {
        var rateLimitKey = $"ratelimit:{operation}:{userId}";
        var allowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, window);
        if (allowed)
        {
            return true;
        }

        // Gửi lỗi realtime giúp client biết thao tác bị từ chối do vượt tần suất.
        await SendClientErrorAsync("too_many_requests", rejectMessage);
        return false;
    }
}
