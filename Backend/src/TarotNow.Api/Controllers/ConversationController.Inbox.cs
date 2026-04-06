using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Chat.Queries.GetUnreadTotal;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Tạo mới hoặc mở lại conversation active theo cặp user-reader.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConversationBody body)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new CreateConversationCommand
        {
            UserId = userId,
            ReaderId = body.ReaderId,
            SlaHours = body.SlaHours ?? 12
        });

        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách conversation (inbox) theo tab active/completed/all.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string tab = "active")
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new ListConversationsQuery
        {
            UserId = userId,
            Tab = tab,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Lấy tổng số tin nhắn chưa đọc của người dùng hiện tại (cho badge thông báo).
    /// </summary>
    [HttpGet("unread-total")]
    public async Task<IActionResult> GetUnreadTotal()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new GetUnreadTotalQuery
        {
            UserId = userId
        });

        return Ok(result);
    }

    /// <summary>
    /// User hủy/xóa cuộc trò chuyện pending trước khi gửi câu hỏi đầu tiên.
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelPending(string id)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new CancelPendingConversationCommand
        {
            ConversationId = id,
            RequesterId = userId
        });

        await TryBroadcastConversationUpdatedAsync(id, "cancelled");
        return Ok(result);
    }
}
