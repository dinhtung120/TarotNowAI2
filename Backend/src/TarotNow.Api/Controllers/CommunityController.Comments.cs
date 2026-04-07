using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Community.Commands.AddComment;
using TarotNow.Application.Features.Community.Queries.GetComments;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
        [HttpPost("posts/{postId}/comments")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> AddComment(string postId, [FromBody] CommunityAddCommentRequest body)
    {
        var result = await _mediator.Send(new AddCommentCommand
        {
            PostId = postId,
            AuthorId = GetRequiredUserId(),
            Content = body.Content
        });

        return Ok(result);
    }

        [HttpGet("posts/{postId}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 50);

        var result = await _mediator.Send(new GetCommentsQuery
        {
            PostId = postId,
            ViewerId = User.GetUserIdOrNull(),
            Page = normalizedPage,
            PageSize = normalizedPageSize
        });

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / normalizedPageSize);
        return Ok(new
        {
            items = result.Items,
            totalCount = result.TotalCount,
            page = normalizedPage,
            pageSize = normalizedPageSize,
            totalPages
        });
    }
}
