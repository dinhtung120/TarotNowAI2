using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Join connection hiện tại vào conversation group.
    /// Luồng xử lý: xác thực user, kiểm tra quyền truy cập conversation, join group và broadcast member_joined.
    /// </summary>
    /// <param name="conversationId">Id hội thoại cần tham gia.</param>
    public async Task JoinConversation(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            // Chặn join khi không parse được user id từ kết nối.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        try
        {
            var validationError = await ValidateJoinConversationAsync(conversationId, userGuid);
            if (validationError != null)
            {
                // Trả lỗi chi tiết cho client khi conversation không tồn tại hoặc không đủ quyền.
                await SendClientErrorAsync(validationError);
                return;
            }

            await AddConnectionToConversationGroupsAsync(conversationId);
            LogConversationJoined(conversationId, userGuid);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "[ChatHub] JoinConversation failed. ConversationId={ConversationId}, UserId={UserId}",
                conversationId,
                userGuid);
            await SendClientErrorAsync("Unable to join conversation right now.");
        }
    }

    /// <summary>
    /// Rời conversation group hiện tại.
    /// </summary>
    /// <param name="conversationId">Id hội thoại cần rời.</param>
    public async Task LeaveConversation(string conversationId)
    {
        var userId = GetUserId();

        // Gỡ connection khỏi group để ngừng nhận message realtime của conversation này.
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConversationGroup(conversationId));

        _logger.LogInformation(
            "[ChatHub] User {UserId} left conversation {ConversationId}",
            userId,
            conversationId);
    }
}
