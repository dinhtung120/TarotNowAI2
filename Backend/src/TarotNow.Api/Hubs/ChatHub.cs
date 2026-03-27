using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Hubs;

[Authorize]
public partial class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    private string? GetUserId() => Context.User.GetUserIdOrNull()?.ToString();

    private bool TryGetUserGuid(out Guid userGuid)
    {
        return Guid.TryParse(GetUserId(), out userGuid);
    }

    private Task SendClientErrorAsync(string message)
    {
        return Clients.Caller.SendAsync("Error", message);
    }
}
