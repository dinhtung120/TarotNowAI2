using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;
using TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
        [HttpPost("{id}/complete/request")]
    public async Task<IActionResult> RequestComplete(string id)
    {
        if (TryGetUserId(out var requesterId) == false)
        {
            return Unauthorized();
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
    public async Task<IActionResult> RespondComplete(string id, [FromBody] ConversationCompleteRespondBody body)
    {
        if (TryGetUserId(out var requesterId) == false)
        {
            return Unauthorized();
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
