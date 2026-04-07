using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Chat.Queries.GetUnreadTotal;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
        [HttpPost]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> Create([FromBody] CreateConversationBody body)
    {
        if (!TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new CreateConversationCommand
        {
            UserId = userId,
            ReaderId = body.ReaderId,
            SlaHours = body.SlaHours ?? 12
        });

        return Ok(result);
    }

        [HttpGet]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string tab = "active")
    {
        if (!TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new ListConversationsQuery
        {
            UserId = userId,
            Tab = tab,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }

        [HttpGet("unread-total")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> GetUnreadTotal()
    {
        if (!TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new GetUnreadTotalQuery
        {
            UserId = userId
        });

        return Ok(result);
    }

        [HttpPost("{id}/cancel")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> CancelPending(string id)
    {
        if (!TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new CancelPendingConversationCommand
        {
            ConversationId = id,
            RequesterId = userId
        });

        await TryBroadcastConversationUpdatedAsync(id, "cancelled");
        return Ok(result);
    }
}
