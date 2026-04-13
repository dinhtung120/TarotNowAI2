using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common.Realtime;

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

        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            RealtimeEventNames.TypingStarted,
            new
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

        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            RealtimeEventNames.TypingStopped,
            new
            {
                conversationId,
                userId = userGuid,
                at = DateTime.UtcNow
            });
    }
}
