using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Community.Commands.CreatePost;
using TarotNow.Application.Features.Community.Commands.DeletePost;
using TarotNow.Application.Features.Community.Commands.UpdatePost;
using TarotNow.Application.Features.Community.Commands.UploadPostImage;
using TarotNow.Application.Features.Community.Queries.GetFeed;
using TarotNow.Application.Features.Community.Queries.GetPostDetail;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
    /// <summary>
    /// Tạo bài viết cộng đồng mới cho người dùng hiện tại.
    /// </summary>
    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostBody body)
    {
        var result = await _mediator.Send(new CreatePostCommand
        {
            AuthorId = GetRequiredUserId(),
            Content = body.Content,
            Visibility = body.Visibility
        });

        return CreatedAtAction(nameof(GetPostDetail), new { id = result.Id }, result);
    }

    /// <summary>
    /// Upload ảnh cho nội dung bài viết cộng đồng.
    /// </summary>
    [HttpPost("images")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { success = false, message = "File rỗng" });
        }

        using var stream = file.OpenReadStream();
        var url = await _mediator.Send(new UploadPostImageCommand
        {
            ImageStream = stream,
            FileName = file.FileName,
            ContentType = file.ContentType
        });

        return Ok(new { success = true, url });
    }

    /// <summary>
    /// Lấy danh sách bài viết cộng đồng theo bộ lọc và phân trang.
    /// </summary>
    [HttpGet("posts")]
    public async Task<IActionResult> GetFeed(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? authorId = null,
        [FromQuery] string? visibility = null)
    {
        var (items, total) = await _mediator.Send(new GetFeedQuery
        {
            ViewerId = GetRequiredUserId(),
            Page = page,
            PageSize = pageSize,
            AuthorFilter = authorId,
            VisibilityFilter = visibility
        });

        return Ok(new { success = true, data = items, metadata = new { totalCount = total, page, pageSize } });
    }

    /// <summary>
    /// Lấy chi tiết một bài viết cộng đồng theo định danh.
    /// </summary>
    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPostDetail(string id)
    {
        var post = await _mediator.Send(new GetPostDetailQuery
        {
            PostId = id,
            ViewerId = GetRequiredUserId()
        });

        return Ok(post);
    }

    /// <summary>
    /// Cập nhật nội dung bài viết cộng đồng do chính người dùng tạo.
    /// </summary>
    [HttpPut("posts/{id}")]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostBody body)
    {
        await _mediator.Send(new UpdatePostCommand
        {
            PostId = id,
            AuthorId = GetRequiredUserId(),
            Content = body.Content
        });

        return NoContent();
    }

    /// <summary>
    /// Xóa mềm bài viết cộng đồng theo quyền tác giả hoặc quản trị.
    /// </summary>
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
        await _mediator.Send(new DeletePostCommand
        {
            PostId = id,
            RequesterId = GetRequiredUserId(),
            RequesterRole = role
        });

        return NoContent();
    }
}
