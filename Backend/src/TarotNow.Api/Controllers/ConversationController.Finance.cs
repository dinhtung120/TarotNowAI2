using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;
using TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;
using TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Reader tạo yêu cầu cộng tiền trong conversation.
    /// </summary>
    [HttpPost("{id}/add-money/request")]
    [Authorize(Roles = "tarot_reader")]
    public async Task<IActionResult> RequestAddMoney(string id, [FromBody] ConversationAddMoneyRequestBody body)
    {
        if (TryGetUserId(out var readerId) == false)
        {
            return Unauthorized();
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
    /// User phản hồi yêu cầu cộng tiền trong conversation.
    /// </summary>
    [HttpPost("{id}/add-money/respond")]
    public async Task<IActionResult> RespondAddMoney(string id, [FromBody] ConversationAddMoneyRespondBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            return Unauthorized();
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
    /// Mở tranh chấp cho conversation.
    /// </summary>
    [HttpPost("{id}/dispute")]
    public async Task<IActionResult> OpenDispute(string id, [FromBody] ConversationDisputeBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            return Unauthorized();
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
