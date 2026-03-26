using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Features.Escrow.Queries.GetEscrowStatus;

namespace TarotNow.Api.Controllers;

public partial class EscrowController
{
    /// <summary>
    /// Adds follow-up paid question into escrow flow.
    /// </summary>
    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new AddQuestionCommand
        {
            UserId = userId.Value,
            ConversationRef = body.ConversationRef,
            AmountDiamond = body.AmountDiamond,
            ProposalMessageRef = body.ProposalMessageRef,
            IdempotencyKey = body.IdempotencyKey
        };

        var itemId = await _mediator.Send(command);
        return Ok(new { success = true, itemId });
    }

    /// <summary>
    /// Returns current escrow status by conversation identifier.
    /// </summary>
    [HttpGet("{conversationId}")]
    public async Task<IActionResult> GetStatus(string conversationId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var query = new GetEscrowStatusQuery
        {
            ConversationRef = conversationId,
            RequesterId = userId.Value
        };

        var result = await _mediator.Send(query);
        return result == null ? NotFound() : Ok(result);
    }
}
