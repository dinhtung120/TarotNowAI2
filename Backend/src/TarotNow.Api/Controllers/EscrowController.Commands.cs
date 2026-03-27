using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Escrow.Commands.AcceptOffer;
using TarotNow.Application.Features.Escrow.Commands.ConfirmRelease;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Features.Escrow.Commands.ReaderReply;

namespace TarotNow.Api.Controllers;

public partial class EscrowController
{
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptOffer([FromBody] AcceptOfferBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new AcceptOfferCommand
        {
            UserId = userId.Value,
            ReaderId = body.ReaderId,
            ConversationRef = body.ConversationRef,
            AmountDiamond = body.AmountDiamond,
            ProposalMessageRef = body.ProposalMessageRef,
            IdempotencyKey = body.IdempotencyKey
        };

        var itemId = await _mediator.Send(command);
        return Ok(new { success = true, itemId });
    }

    [HttpPost("reply")]
    public async Task<IActionResult> ReaderReply([FromBody] ReaderReplyBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new ReaderReplyCommand
        {
            ItemId = body.ItemId,
            ReaderId = userId.Value
        };

        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmRelease([FromBody] ConfirmReleaseBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new ConfirmReleaseCommand
        {
            ItemId = body.ItemId,
            UserId = userId.Value
        };

        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    [HttpPost("dispute")]
    public async Task<IActionResult> OpenDispute([FromBody] OpenDisputeBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new OpenDisputeCommand
        {
            ItemId = body.ItemId,
            UserId = userId.Value,
            Reason = body.Reason
        };

        await _mediator.Send(command);
        return Ok(new { success = true });
    }
}
