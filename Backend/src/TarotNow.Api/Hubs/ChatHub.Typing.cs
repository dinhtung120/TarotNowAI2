using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Broadcast trạng thái bắt đầu gõ trong conversation.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh typing event.</param>
    public async Task TypingStarted(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            // Chặn typing event từ kết nối không xác thực.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        // Gửi typing.started để các participant hiển thị trạng thái đang nhập.
        await Clients.Group(ConversationGroup(conversationId)).SendAsync("typing.started", new
        {
            conversationId,
            userId = userGuid,
            at = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Broadcast trạng thái dừng gõ trong conversation.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh typing event.</param>
    public async Task TypingStopped(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            // Chặn typing event từ kết nối không xác thực.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        // Gửi typing.stopped để client tắt indicator nhập liệu.
        await Clients.Group(ConversationGroup(conversationId)).SendAsync("typing.stopped", new
        {
            conversationId,
            userId = userGuid,
            at = DateTime.UtcNow
        });
    }
}
