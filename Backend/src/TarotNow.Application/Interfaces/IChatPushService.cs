namespace TarotNow.Application.Interfaces;

using TarotNow.Application.Common;

public interface IChatPushService
{
    Task BroadcastMessageAsync(string conversationId, ChatMessageDto message, CancellationToken ct = default);
    Task BroadcastConversationUpdatedAsync(string conversationId, string updateType, CancellationToken ct = default);
    Task BroadcastCallEndedAsync(string conversationId, CallSessionDto session, string reason, CancellationToken ct = default);
}
