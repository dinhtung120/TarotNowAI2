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
    /// <summary>
    /// Thêm bình luận mới cho bài viết.
    /// Luồng xử lý: lấy user hiện tại làm author comment và gửi command thêm bình luận.
    /// </summary>
    /// <param name="postId">Id bài viết nhận bình luận.</param>
    /// <param name="body">Payload nội dung bình luận.</param>
    /// <returns>Kết quả comment sau khi tạo thành công.</returns>
    [HttpPost("posts/{postId}/comments")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> AddComment(string postId, [FromBody] CommunityAddCommentRequest body)
    {
        var result = await _mediator.Send(new AddCommentCommand
        {
            PostId = postId,
            AuthorId = GetRequiredUserId(),
            Content = body.Content,
            ContextDraftId = body.ContextDraftId
        });

        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách bình luận của bài viết theo phân trang.
    /// Luồng xử lý: chuẩn hóa page/pageSize, gửi query và trả metadata total pages.
    /// </summary>
    /// <param name="postId">Id bài viết cần lấy bình luận.</param>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số bình luận mỗi trang.</param>
    /// <returns>Danh sách bình luận và metadata phân trang.</returns>
    [HttpGet("posts/{postId}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // Chuẩn hóa tham số phân trang để tránh giá trị âm/0 làm hỏng truy vấn.
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 50);

        var result = await _mediator.Send(new GetCommentsQuery
        {
            PostId = postId,
            ViewerId = User.GetUserIdOrNull(),
            Page = normalizedPage,
            PageSize = normalizedPageSize
        });

        // Tính totalPages ở API để client không phải lặp lại công thức và giảm sai lệch hiển thị.
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
