using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Hubs;

[Authorize]
public class PresenceHub : Hub
{
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly ILogger<PresenceHub> _logger;

    public PresenceHub(IUserPresenceTracker presenceTracker, ILogger<PresenceHub> logger)
    {
        _presenceTracker = presenceTracker;
        _logger = logger;
    }

    private string? GetUserId() => Context.User.GetUserIdOrNull()?.ToString();

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.MarkConnected(userId, Context.ConnectionId);

            
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            
            
            await Clients.All.SendAsync("UserStatusChanged", userId, "online");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} connected. ConnectionId: {ConnectionId}",
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.MarkDisconnected(userId, Context.ConnectionId);

            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} disconnected. Reason: {Reason}",
            userId,
            exception?.Message ?? "normal");

        await base.OnDisconnectedAsync(exception);
    }

        public Task Heartbeat()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.RecordHeartbeat(userId);
            _logger.LogDebug("[PresenceHub] User {UserId} heartbeat received.", userId);
        }

        return Task.CompletedTask;
    }
}
