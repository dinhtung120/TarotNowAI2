using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Community.Commands.ReportPost;
using TarotNow.Application.Features.Community.Commands.ToggleReaction;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
        [HttpPost("posts/{id}/reactions")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> ToggleReaction(string id, [FromBody] ToggleReactionBody body)
    {
        await _mediator.Send(new ToggleReactionCommand
        {
            PostId = id,
            UserId = GetRequiredUserId(),
            ReactionType = body.Type
        });

        return Ok(new { success = true });
    }

        [HttpPost("posts/{id}/reports")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> ReportPost(string id, [FromBody] ReportPostBody body)
    {
        var result = await _mediator.Send(new ReportPostCommand
        {
            PostId = id,
            ReporterId = GetRequiredUserId(),
            ReasonCode = body.ReasonCode,
            Description = body.Description
        });

        return Ok(new { success = true, reportId = result.Id });
    }
}
