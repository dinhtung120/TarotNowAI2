using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Community.Commands.AddComment;
using TarotNow.Application.Features.Community.Queries.GetComments;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
    /// <summary>
    /// Thêm bình luận mới vào một bài viết cộng đồng.
    /// </summary>
    [HttpPost("posts/{postId}/comments")]
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

    /// <summary>
    /// Lấy danh sách bình luận có phân trang của một bài viết.
    /// </summary>
    [HttpGet("posts/{postId}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetCommentsQuery
        {
            PostId = postId,
            ViewerId = User.GetUserIdOrNull(),
            Page = page,
            PageSize = pageSize
        });

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);
        return Ok(new { items = result.Items, totalCount = result.TotalCount, page, pageSize, totalPages });
    }
}
