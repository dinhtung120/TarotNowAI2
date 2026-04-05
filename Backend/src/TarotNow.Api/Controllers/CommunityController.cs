/*
 * ===================================================================
 * FILE: CommunityController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH:
 *   Controller phục vụ tính năng Mạng Xã Hội (Community) của App.
 *   Nơi người dùng lên kể chuyện, thả react và tố cáo nhau.
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Community.Commands.CreatePost;
using TarotNow.Application.Features.Community.Commands.DeletePost;
using TarotNow.Application.Features.Community.Commands.ReportPost;
using TarotNow.Application.Features.Community.Commands.ToggleReaction;
using TarotNow.Application.Features.Community.Commands.UpdatePost;
using TarotNow.Application.Features.Community.Queries.GetFeed;
using TarotNow.Application.Features.Community.Queries.GetPostDetail;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Community)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] // Bắt buộc đăng nhập mới được bon chen
public class CommunityController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostBody body)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var command = new CreatePostCommand
        {
            AuthorId = userId,
            Content = body.Content,
            Visibility = body.Visibility
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPostDetail), new { id = result.Id }, result);
    }

    [HttpPost("images")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        if (file == null || file.Length == 0)
            return BadRequest(new { success = false, message = "File rỗng" });

        using var stream = file.OpenReadStream();
        var command = new TarotNow.Application.Features.Community.Commands.UploadPostImage.UploadPostImageCommand
        {
            ImageStream = stream,
            FileName = file.FileName,
            ContentType = file.ContentType
        };

        var url = await _mediator.Send(command);
        return Ok(new { success = true, url });
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetFeed(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? authorId = null,
        [FromQuery] string? visibility = null)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var query = new GetFeedQuery
        {
            ViewerId = userId,
            Page = page,
            PageSize = pageSize,
            AuthorFilter = authorId,
            VisibilityFilter = visibility
        };

        var (items, total) = await _mediator.Send(query);
        return Ok(new
        {
            success = true,
            data = items,
            metadata = new { totalCount = total, page, pageSize }
        });
    }

    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPostDetail(string id)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var query = new GetPostDetailQuery { PostId = id, ViewerId = userId };
        var post = await _mediator.Send(query);
        return Ok(post);
    }

    [HttpPut("posts/{id}")]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostBody body)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var command = new UpdatePostCommand
        {
            PostId = id,
            AuthorId = userId,
            Content = body.Content
        };

        await _mediator.Send(command);
        return NoContent(); // Code 204: Vui vẻ không nói gì thêm
    }

    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
        
        var command = new DeletePostCommand
        {
            PostId = id,
            RequesterId = userId,
            RequesterRole = role
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("posts/{id}/reactions")]
    public async Task<IActionResult> ToggleReaction(string id, [FromBody] ToggleReactionBody body)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var command = new ToggleReactionCommand
        {
            PostId = id,
            UserId = userId,
            ReactionType = body.Type
        };

        await _mediator.Send(command);
        return Ok(new { success = true });
    }



    [HttpPost("posts/{id}/reports")]
    public async Task<IActionResult> ReportPost(string id, [FromBody] ReportPostBody body)
    {
        var userId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var command = new ReportPostCommand
        {
            PostId = id,
            ReporterId = userId,
            ReasonCode = body.ReasonCode,
            Description = body.Description
        };

        var result = await _mediator.Send(command);
        return Ok(new { success = true, reportId = result.Id });
    }



    [HttpPost("posts/{postId}/comments")]
    public async Task<IActionResult> AddComment(string postId, [FromBody] TarotNow.Api.Contracts.Requests.CommunityAddCommentRequest body)
    {
        var userId = User.GetUserIdOrNull();
        if (userId == null || userId == Guid.Empty)
            return Unauthorized();

        var command = new TarotNow.Application.Features.Community.Commands.AddComment.AddCommentCommand
        {
            PostId = postId,
            AuthorId = userId.Value,
            Content = body.Content
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("posts/{postId}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new TarotNow.Application.Features.Community.Queries.GetComments.GetCommentsQuery
        {
            PostId = postId,
            ViewerId = User.GetUserIdOrNull(),
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(new
        {
            items = result.Items,
            totalCount = result.TotalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize)
        });
    }
}
