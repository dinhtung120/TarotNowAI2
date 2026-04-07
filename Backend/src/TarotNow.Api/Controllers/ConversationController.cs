using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Api.Extensions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Conversations)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
[EnableRateLimiting("auth-session")]
public partial class ConversationController : ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly IConversationRepository ConversationRepository;
    protected readonly IHubContext<ChatHub> ChatHubContext;

    public ConversationController(
        IMediator mediator,
        IConversationRepository conversationRepository,
        IHubContext<ChatHub> chatHubContext)
    {
        Mediator = mediator;
        ConversationRepository = conversationRepository;
        ChatHubContext = chatHubContext;
    }

    protected bool TryGetUserId(out Guid userId)
    {
        return User.TryGetUserId(out userId);
    }

    protected static string ConversationGroup(string conversationId) => $"conversation:{conversationId}";
    protected static string UserGroup(string userId) => $"user:{userId}";

    protected async Task TryBroadcastConversationUpdatedAsync(string conversationId, string type)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        try
        {
            var conversation = await ConversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
            {
                return;
            }

            await ChatHubContext.Clients.Groups(
                UserGroup(conversation.UserId),
                UserGroup(conversation.ReaderId)).SendAsync("conversation.updated", new
            {
                conversationId,
                type,
                at = DateTime.UtcNow
            });
        }
        catch
        {
            
        }
    }
}
