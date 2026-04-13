using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.AcceptConversation;
using TarotNow.Application.Features.Chat.Commands.RejectConversation;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Reader chấp nhận hội thoại chờ xử lý.
    /// Luồng xử lý: xác thực reader, gửi command accept.
    /// </summary>
    /// <param name="id">Id hội thoại cần chấp nhận.</param>
    /// <returns>Kết quả accept conversation hoặc unauthorized khi thiếu reader id.</returns>
    [HttpPost("{id}/accept")]
    [Authorize(Roles = "tarot_reader")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> AcceptConversation(string id)
    {
        if (TryGetUserId(out var readerId) == false)
        {
            // Chặn thao tác accept khi không xác định được danh tính reader.
            return this.UnauthorizedProblem();
        }

        // Mapping command rõ ràng để handler xác thực quyền và trạng thái hội thoại.
        var result = await Mediator.Send(new AcceptConversationCommand
        {
            ConversationId = id,
            ReaderId = readerId
        });

        return Ok(result);
    }

    /// <summary>
    /// Reader từ chối hội thoại chờ xử lý.
    /// Luồng xử lý: xác thực reader, gửi command reject kèm lý do.
    /// </summary>
    /// <param name="id">Id hội thoại cần từ chối.</param>
    /// <param name="body">Payload lý do từ chối.</param>
    /// <returns>Kết quả reject conversation hoặc unauthorized khi thiếu reader id.</returns>
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "tarot_reader")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RejectConversation(string id, [FromBody] ConversationRejectBody body)
    {
        if (TryGetUserId(out var readerId) == false)
        {
            // Chặn thao tác reject khi chủ thể không hợp lệ để đảm bảo audit đúng người thực hiện.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RejectConversationCommand
        {
            ConversationId = id,
            ReaderId = readerId,
            Reason = body.Reason
        });

        return Ok(result);
    }
}
