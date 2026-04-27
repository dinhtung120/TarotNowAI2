using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Community.Commands.CreatePost;
using TarotNow.Application.Features.Community.Commands.DeletePost;
using TarotNow.Application.Features.Community.Commands.UpdatePost;
using TarotNow.Application.Features.Community.Queries.GetFeed;
using TarotNow.Application.Features.Community.Queries.GetPostDetail;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
    /// <summary>
    /// Tạo bài viết cộng đồng mới.
    /// Luồng xử lý: lấy user hiện tại làm author, gửi command tạo post, trả CreatedAtAction.
    /// </summary>
    /// <param name="body">Payload tạo bài viết.</param>
    /// <returns>Post vừa tạo với HTTP 201.</returns>
    [HttpPost("posts")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostBody body)
    {
        // Mapping rõ ràng từ DTO sang command để handler tập trung xử lý rule nghiệp vụ.
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new CreatePostCommand
        {
            AuthorId = GetRequiredUserId(),
            Content = body.Content,
            Visibility = body.Visibility,
            ContextDraftId = body.ContextDraftId
        });

        return CreatedAtAction(nameof(GetPostDetail), new { id = result.Id }, result);
    }

    /// <summary>
    /// Lấy feed bài viết cộng đồng có phân trang và bộ lọc.
    /// Luồng xử lý: gửi query với ngữ cảnh viewer hiện tại để backend áp dụng rule hiển thị.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số mục mỗi trang.</param>
    /// <param name="authorId">Bộ lọc tác giả tùy chọn.</param>
    /// <param name="visibility">Bộ lọc mức hiển thị tùy chọn.</param>
    /// <returns>Dữ liệu feed và metadata phân trang.</returns>
    [HttpGet("posts")]
    public async Task<IActionResult> GetFeed(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? authorId = null,
        [FromQuery] string? visibility = null)
    {
        // ViewerId được truyền xuống để handler xử lý đúng quyền xem theo visibility.
        var (items, total) = await _mediator.SendWithRequestCancellation(HttpContext, new GetFeedQuery
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
    /// Lấy chi tiết một bài viết cộng đồng.
    /// </summary>
    /// <param name="id">Id bài viết.</param>
    /// <returns>Thông tin chi tiết bài viết.</returns>
    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPostDetail(string id)
    {
        var post = await _mediator.SendWithRequestCancellation(HttpContext, new GetPostDetailQuery
        {
            PostId = id,
            ViewerId = GetRequiredUserId()
        });

        return Ok(post);
    }

    /// <summary>
    /// Cập nhật nội dung một bài viết.
    /// Luồng xử lý: gắn author hiện tại để handler xác thực quyền sửa bài.
    /// </summary>
    /// <param name="id">Id bài viết cần cập nhật.</param>
    /// <param name="body">Nội dung cập nhật.</param>
    /// <returns>HTTP 204 khi cập nhật thành công.</returns>
    [HttpPut("posts/{id}")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostBody body)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, new UpdatePostCommand
        {
            PostId = id,
            AuthorId = GetRequiredUserId(),
            Content = body.Content
        });

        return NoContent();
    }

    /// <summary>
    /// Xóa một bài viết cộng đồng.
    /// Luồng xử lý: lấy role hiện tại, gửi command xóa để handler áp dụng đúng rule quyền xóa.
    /// </summary>
    /// <param name="id">Id bài viết cần xóa.</param>
    /// <returns>HTTP 204 khi xóa thành công.</returns>
    [HttpDelete("posts/{id}")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> DeletePost(string id)
    {
        // Role được gửi xuống để hỗ trợ nhánh quyền admin/moderator khác với author thường.
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
        await _mediator.SendWithRequestCancellation(HttpContext, new DeletePostCommand
        {
            PostId = id,
            RequesterId = GetRequiredUserId(),
            RequesterRole = role
        });

        return NoContent();
    }
}
