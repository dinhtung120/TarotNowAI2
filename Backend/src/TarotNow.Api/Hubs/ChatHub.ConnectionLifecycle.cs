namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        _logger.LogInformation(
            "[ChatHub] User {UserId} connected. ConnectionId: {ConnectionId}",
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        _logger.LogInformation(
            "[ChatHub] User {UserId} disconnected. Reason: {Reason}",
            userId,
            exception?.Message ?? "normal");

        await base.OnDisconnectedAsync(exception);
    }
}
