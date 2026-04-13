using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Đánh dấu đã đọc toàn bộ tin nhắn trong conversation.
    /// Luồng xử lý: xác thực user, gọi core mark-read, bắt lỗi business/hạ tầng và phản hồi realtime.
    /// </summary>
    /// <param name="conversationId">Id conversation cần đánh dấu đã đọc.</param>
    public async Task MarkRead(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            // Chặn thao tác mark read khi user chưa xác thực hợp lệ.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        try
        {
            await MarkReadCoreAsync(conversationId, userGuid);
        }
        catch (BadRequestException ex)
        {
            // Trả lỗi nghiệp vụ do input không hợp lệ.
            await SendClientErrorAsync(ex.Message);
        }
        catch (NotFoundException ex)
        {
            // Trả lỗi khi conversation không tồn tại hoặc user không còn quyền.
            await SendClientErrorAsync(ex.Message);
        }
        catch (Exception ex)
        {
            // Lỗi không mong đợi được log chi tiết để điều tra.
            _logger.LogError(
                ex,
                "[ChatHub] MarkRead failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await SendClientErrorAsync("Unable to mark messages as read. Please try again.");
        }
    }

    /// <summary>
    /// Thực thi luồng mark-read và broadcast sự kiện liên quan.
    /// </summary>
    /// <param name="conversationId">Id conversation cần đánh dấu đọc.</param>
    /// <param name="userGuid">User id thực hiện mark-read.</param>
    private async Task MarkReadCoreAsync(string conversationId, Guid userGuid)
    {
        await _mediator.Send(new MarkMessagesReadCommand
        {
            ConversationId = conversationId,
            ReaderId = userGuid
        });

        var payload = new
        {
            userId = userGuid,
            conversationId,
            readAt = DateTime.UtcNow
        };

        var groupKey = ConversationGroup(conversationId);
        // Phát sự kiện message.read để các client đồng bộ trạng thái đã đọc theo thời gian thực.
        await Clients.Group(groupKey).SendAsync("message.read", payload);
    }
}
