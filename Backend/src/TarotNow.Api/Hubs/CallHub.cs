using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TarotNow.Api.Constants;
using TarotNow.Application.Interfaces;
using System.Security.Claims;

namespace TarotNow.Api.Hubs;

[Authorize(Policy = ApiAuthorizationPolicies.AuthenticatedUser)]
public partial class CallHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<CallHub> _logger;
    private readonly ICacheService _cacheService;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public CallHub(
        IMediator mediator,
        ILogger<CallHub> logger,
        ICacheService cacheService,
        IHubContext<ChatHub> chatHubContext)
    {
        _mediator = mediator;
        _logger = logger;
        _cacheService = cacheService;
        _chatHubContext = chatHubContext;
    }

        protected string GetUserId()
    {
        return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

        protected bool TryGetUserGuid(out Guid userId)
    {
        return Guid.TryParse(GetUserId(), out userId);
    }

        protected string ConversationGroup(string conversationId) => $"conversation:{conversationId}";

        protected async Task SendClientErrorAsync(string errorKey, string message)
    {
        await Clients.Caller.SendAsync("call.error", new { errorKey, message });
    }
}
