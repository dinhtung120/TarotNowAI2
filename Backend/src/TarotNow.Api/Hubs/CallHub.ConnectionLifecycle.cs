using TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

namespace TarotNow.Api.Hubs;

internal static class CallConversationStatuses
{
    public const string Ongoing = "ongoing";
    public const string Pending = "pending";
    public const string AwaitingAcceptance = "awaiting_acceptance";

    public static readonly string[] ActiveConversationStates =
    [
        Ongoing,
        Pending,
        AwaitingAcceptance
    ];
}

public partial class CallHub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        RegisterConnection(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        var conversationIds = await GetActiveConversationIdsAsync(userId);
        RememberConversationAccess(userId, conversationIds);
        foreach (var conversationId in conversationIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ConversationGroup(conversationId));
        }

        await base.OnConnectedAsync();
    }

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
