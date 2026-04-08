using TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

namespace TarotNow.Api.Hubs;

// Tập trạng thái conversation được xem là còn hoạt động cho luồng cuộc gọi.
internal static class CallConversationStatuses
{
    // Conversation đang diễn ra.
    public const string Ongoing = "ongoing";
    // Conversation đang chờ xử lý.
    public const string Pending = "pending";
    // Conversation đang chờ bên còn lại chấp nhận.
    public const string AwaitingAcceptance = "awaiting_acceptance";

    // Danh sách trạng thái active dùng cho truy vấn nối group realtime.
    public static readonly string[] ActiveConversationStates =
    [
        Ongoing,
        Pending,
        AwaitingAcceptance
    ];
}

public partial class CallHub
{
    /// <summary>
    /// Xử lý khi kết nối hub mới được thiết lập.
    /// Luồng xử lý: xác thực user id, đăng ký connection, join group user + các group conversation đang active.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            // Không có user id thì không join group để tránh rò rỉ sự kiện realtime.
            return;
        }

        RegisterConnection(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");

        // Nạp danh sách conversation active để kết nối mới nhận được signaling đúng ngữ cảnh.
        var conversationIds = await GetActiveConversationIdsAsync(userId);
        RememberConversationAccess(userId, conversationIds);
        foreach (var conversationId in conversationIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ConversationGroup(conversationId));
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Xử lý khi kết nối hub bị ngắt.
    /// Luồng xử lý: gỡ connection khỏi state và chạy cleanup trễ để hấp thụ reconnect tạm thời.
    /// </summary>
    /// <param name="exception">Exception ngắt kết nối nếu có.</param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId) == false)
        {
            UnregisterConnection(userId, Context.ConnectionId);
            await DelayCleanupForTransientDisconnectAsync(userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Lấy danh sách conversation active mà user đang tham gia.
    /// </summary>
    /// <param name="userId">User id dạng chuỗi.</param>
    /// <returns>Danh sách conversation id active để join group/broadcast.</returns>
    private async Task<IReadOnlyList<string>> GetActiveConversationIdsAsync(string userId)
    {
        return await _mediator.Send(new GetParticipantConversationIdsQuery
        {
            ParticipantId = userId,
            MaxCount = 200,
            Statuses = CallConversationStatuses.ActiveConversationStates
        });
    }
}
