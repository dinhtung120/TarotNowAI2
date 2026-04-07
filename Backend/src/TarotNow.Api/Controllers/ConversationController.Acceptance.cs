using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Chat.Commands.AcceptConversation;
using TarotNow.Application.Features.Chat.Commands.RejectConversation;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
        [HttpPost("{id}/accept")]
    [Authorize(Roles = "tarot_reader")]
    public async Task<IActionResult> AcceptConversation(string id)
    {
        if (TryGetUserId(out var readerId) == false)
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new AcceptConversationCommand
        {
            ConversationId = id,
            ReaderId = readerId
        });

        await TryBroadcastConversationUpdatedAsync(id, "accepted");
        return Ok(result);
    }

        [HttpPost("{id}/reject")]
    [Authorize(Roles = "tarot_reader")]
    public async Task<IActionResult> RejectConversation(string id, [FromBody] ConversationRejectBody body)
    {
        if (TryGetUserId(out var readerId) == false)
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new RejectConversationCommand
        {
            ConversationId = id,
            ReaderId = readerId,
            Reason = body.Reason
        });

        await TryBroadcastConversationUpdatedAsync(id, "rejected");
        return Ok(result);
    }
}
