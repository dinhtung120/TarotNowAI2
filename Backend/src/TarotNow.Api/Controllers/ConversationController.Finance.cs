using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;
using TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;
using TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Reader gửi đề nghị nạp thêm kim cương trong hội thoại.
    /// Luồng xử lý: xác thực reader, gửi command request add-money, broadcast cập nhật.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="body">Payload đề nghị nạp thêm.</param>
    /// <returns>Kết quả request add-money hoặc unauthorized khi thiếu reader id.</returns>
    [HttpPost("{id}/add-money/request")]
    [Authorize(Roles = "tarot_reader")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RequestAddMoney(string id, [FromBody] ConversationAddMoneyRequestBody body)
    {
        if (TryGetUserId(out var readerId) == false)
        {
            // Chặn thao tác tài chính khi không xác định được reader hợp lệ.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RequestConversationAddMoneyCommand
        {
            ConversationId = id,
            ReaderId = readerId,
            AmountDiamond = body.AmountDiamond,
            Description = body.Description,
            IdempotencyKey = body.IdempotencyKey
        });

        await TryBroadcastConversationUpdatedAsync(id, "add_money_requested");
        return Ok(result);
    }

    /// <summary>
    /// Người dùng phản hồi đề nghị nạp thêm kim cương.
    /// Luồng xử lý: xác thực user, gửi command phản hồi, broadcast trạng thái cập nhật.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="body">Payload phản hồi đề nghị nạp thêm.</param>
    /// <returns>Kết quả phản hồi add-money hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost("{id}/add-money/respond")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> RespondAddMoney(string id, [FromBody] ConversationAddMoneyRespondBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            // Chặn phản hồi tài chính từ chủ thể chưa xác thực.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new RespondConversationAddMoneyCommand
        {
            ConversationId = id,
            UserId = userId,
            Accept = body.Accept,
            OfferMessageId = body.OfferMessageId,
            RejectReason = body.RejectReason
        });

        await TryBroadcastConversationUpdatedAsync(id, "add_money_responded");
        return Ok(result);
    }

    /// <summary>
    /// Mở tranh chấp trong ngữ cảnh hội thoại.
    /// Luồng xử lý: xác thực user, gửi command dispute, broadcast trạng thái tranh chấp.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="body">Payload lý do tranh chấp.</param>
    /// <returns>Kết quả mở dispute hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost("{id}/dispute")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> OpenDispute(string id, [FromBody] ConversationDisputeBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            // Chặn tạo dispute nếu không có user id hợp lệ để tránh bản ghi mồ côi.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new OpenConversationDisputeCommand
        {
            ConversationId = id,
            UserId = userId,
            ItemId = body.ItemId,
            Reason = body.Reason
        });

        await TryBroadcastConversationUpdatedAsync(id, "disputed");
        return Ok(result);
    }
}
