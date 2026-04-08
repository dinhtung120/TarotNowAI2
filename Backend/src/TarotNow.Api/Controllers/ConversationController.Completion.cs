using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;
using TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Gửi yêu cầu hoàn tất hội thoại.
    /// Luồng xử lý: xác thực requester, gửi command request complete, broadcast cập nhật trạng thái.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <returns>Kết quả request complete hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost("{id}/complete/request")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RequestComplete(string id)
    {
        if (TryGetUserId(out var requesterId) == false)
        {
            // Chặn yêu cầu hoàn tất khi không xác định được chủ thể thao tác.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RequestConversationCompleteCommand
        {
            ConversationId = id,
            RequesterId = requesterId
        });

        await TryBroadcastConversationUpdatedAsync(id, "complete_requested");
        return Ok(result);
    }

    /// <summary>
    /// Phản hồi yêu cầu hoàn tất hội thoại.
    /// Luồng xử lý: xác thực requester, gửi command phản hồi accept/reject, broadcast trạng thái mới.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="body">Payload quyết định phản hồi hoàn tất.</param>
    /// <returns>Kết quả phản hồi complete hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost("{id}/complete/respond")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RespondComplete(string id, [FromBody] ConversationCompleteRespondBody body)
    {
        if (TryGetUserId(out var requesterId) == false)
        {
            // Nhánh unauthorized bảo vệ thao tác thay đổi trạng thái lifecycle hội thoại.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RespondConversationCompleteCommand
        {
            ConversationId = id,
            RequesterId = requesterId,
            Accept = body.Accept
        });

        await TryBroadcastConversationUpdatedAsync(id, "complete_responded");
        return Ok(result);
    }
}
