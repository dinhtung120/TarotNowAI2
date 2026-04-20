using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Kiểm tra điều kiện join conversation của user.
    /// Luồng xử lý: validate input conversation id, gọi query kiểm tra quyền, map lỗi ra chuỗi thân thiện.
    /// </summary>
    /// <param name="conversationId">Id conversation cần join.</param>
    /// <param name="userGuid">User id của kết nối hiện tại.</param>
    /// <returns>Chuỗi lỗi nếu không hợp lệ; ngược lại <c>null</c>.</returns>
    private async Task<string?> ValidateJoinConversationAsync(string conversationId, Guid userGuid)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            // Chặn sớm input rỗng để không gọi query tốn tài nguyên.
            return "ConversationId is required";
        }

        var accessStatus = await _mediator.Send(new ValidateConversationAccessQuery
        {
            ConversationId = conversationId,
            RequesterId = userGuid
        });
        return MapConversationAccessError(accessStatus);
    }

    /// <summary>
    /// Ánh xạ trạng thái quyền truy cập conversation sang thông điệp lỗi client.
    /// </summary>
    /// <param name="accessStatus">Kết quả validate quyền từ tầng ứng dụng.</param>
    /// <returns>Thông điệp lỗi nếu truy cập bị từ chối; ngược lại <c>null</c>.</returns>
    private static string? MapConversationAccessError(ConversationAccessStatus accessStatus)
    {
        if (accessStatus == ConversationAccessStatus.NotFound)
        {
            return "Conversation not found";
        }

        return accessStatus == ConversationAccessStatus.Forbidden ? "Forbidden" : null;
    }

    /// <summary>
    /// Thêm connection hiện tại vào group conversation.
    /// </summary>
    /// <param name="conversationId">Id conversation cần join group.</param>
    private async Task AddConnectionToConversationGroupsAsync(string conversationId)
    {
        var groupKey = ConversationGroup(conversationId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupKey);
    }

    /// <summary>
    /// Ghi log sự kiện join conversation.
    /// </summary>
    /// <param name="conversationId">Id conversation.</param>
    /// <param name="userGuid">User id vừa join.</param>
    private void LogConversationJoined(string conversationId, Guid userGuid)
    {
        _logger.LogInformation(
            "[ChatHub] User {UserId} joined conversation {ConversationId}",
            userGuid,
            conversationId);
    }
}
