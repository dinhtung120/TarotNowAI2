using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Community.Commands.ReportPost;
using TarotNow.Application.Features.Community.Commands.ToggleReaction;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
    /// <summary>
    /// Bật/tắt reaction của người dùng lên bài viết.
    /// Luồng xử lý: lấy user hiện tại, gửi command toggle reaction, trả cờ success.
    /// </summary>
    /// <param name="id">Id bài viết.</param>
    /// <param name="body">Payload loại reaction.</param>
    /// <returns>Kết quả success của thao tác reaction.</returns>
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

    /// <summary>
    /// Gửi báo cáo vi phạm cho một bài viết.
    /// Luồng xử lý: tạo command report từ payload và trả report id để client theo dõi.
    /// </summary>
    /// <param name="id">Id bài viết bị báo cáo.</param>
    /// <param name="body">Payload lý do và mô tả báo cáo.</param>
    /// <returns>Kết quả success kèm id báo cáo đã tạo.</returns>
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
