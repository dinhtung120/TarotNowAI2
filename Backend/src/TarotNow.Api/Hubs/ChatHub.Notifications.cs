using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Chat.Queries.GetConversationParticipants;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Broadcast sự kiện conversation.updated tới hai participant của conversation.
    /// Luồng xử lý: validate conversation id, lấy participant, gửi event tới user-group tương ứng.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh cập nhật.</param>
    /// <param name="type">Loại cập nhật conversation.</param>
    /// <param name="atUtc">Thời điểm cập nhật theo UTC.</param>
    private async Task BroadcastConversationUpdatedToParticipantsAsync(
        string conversationId,
        string type,
        DateTime atUtc)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            // Bỏ qua broadcast khi conversation id rỗng để tránh sự kiện không định danh.
            return;
        }

        var participants = await _mediator.Send(new GetConversationParticipantsQuery
        {
            ConversationId = conversationId
        });

        if (participants == null)
        {
            // Không có participant hợp lệ thì không thể broadcast sự kiện cập nhật.
            return;
        }

        var userGroups = new[]
        {
            UserGroup(participants.UserId),
            UserGroup(participants.ReaderId)
        };

        // Broadcast theo user-group giúp cả inbox và màn hình chat nhận cập nhật đồng thời.
        await Clients.Groups(userGroups).SendAsync("conversation.updated", new
        {
            conversationId,
            type,
            at = atUtc
        });
    }
}
