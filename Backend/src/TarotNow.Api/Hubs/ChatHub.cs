using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;
using TarotNow.Api.Realtime;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Hubs;

[Authorize]
public partial class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(
        IMediator mediator,
        IConversationRepository conversationRepository,
        IUserPresenceTracker presenceTracker,
        ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _conversationRepository = conversationRepository;
        _presenceTracker = presenceTracker;
        _logger = logger;
    }

    private string? GetUserId() => Context.User.GetUserIdOrNull()?.ToString();

    private bool TryGetUserGuid(out Guid userGuid)
    {
        return Guid.TryParse(GetUserId(), out userGuid);
    }

    private static string ConversationGroup(string conversationId) => $"conversation:{conversationId}";
    private static string UserGroup(string userId) => $"user:{userId}";

    private Task SendClientErrorAsync(string message)
    {
        return Clients.Caller.SendAsync("Error", message);
    }
}
