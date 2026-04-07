using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;
using TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
        [HttpPost("{id}/complete/request")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RequestComplete(string id)
    {
        if (TryGetUserId(out var requesterId) == false)
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RequestConversationCompleteCommand
        {
            ConversationId = id,
            RequesterId = requesterId
        });

        await TryBroadcastConversationUpdatedAsync(id, "complete_requested");
        return Ok(result);
    }

        [HttpPost("{id}/complete/respond")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RespondComplete(string id, [FromBody] ConversationCompleteRespondBody body)
    {
        if (TryGetUserId(out var requesterId) == false)
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RespondConversationCompleteCommand
        {
            ConversationId = id,
            RequesterId = requesterId,
            Accept = body.Accept
        });

        await TryBroadcastConversationUpdatedAsync(id, "complete_responded");
        return Ok(result);
    }
}
