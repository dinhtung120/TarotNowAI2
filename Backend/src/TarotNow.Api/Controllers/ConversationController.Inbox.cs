using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Chat.Queries.GetUnreadTotal;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Tạo hội thoại mới giữa user và reader.
    /// Luồng xử lý: xác thực user, gửi command tạo hội thoại, áp dụng SLA mặc định khi client không truyền.
    /// </summary>
    /// <param name="body">Payload tạo hội thoại.</param>
    /// <returns>Thông tin hội thoại vừa tạo hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> Create([FromBody] CreateConversationBody body)
    {
        if (!TryGetUserId(out var userId))
        {
            // Chặn tạo conversation khi không xác định được người khởi tạo.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new CreateConversationCommand
        {
            UserId = userId,
            ReaderId = body.ReaderId,
            // Dùng SLA mặc định 12 giờ để giữ behavior ổn định khi client không gửi trường này.
            SlaHours = body.SlaHours ?? 12
        });

        return Ok(result);
    }

    /// <summary>
    /// Liệt kê danh sách hội thoại theo tab và phân trang.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số hội thoại mỗi trang.</param>
    /// <param name="tab">Tab lọc trạng thái hội thoại.</param>
    /// <returns>Danh sách hội thoại theo điều kiện truy vấn.</returns>
    [HttpGet]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string tab = "active")
    {
        if (!TryGetUserId(out var userId))
        {
            // Chặn truy vấn inbox khi không có user id hợp lệ.
            return this.UnauthorizedProblem();
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
    /// Lấy tổng số hội thoại/tin nhắn chưa đọc của người dùng hiện tại.
    /// </summary>
    /// <returns>Tổng số chưa đọc hoặc unauthorized khi thiếu user id.</returns>
    [HttpGet("unread-total")]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> GetUnreadTotal()
    {
        if (!TryGetUserId(out var userId))
        {
            // Chặn truy cập thống kê unread khi không xác định được chủ thể.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new GetUnreadTotalQuery
        {
            UserId = userId
        });

        return Ok(result);
    }

    /// <summary>
    /// Hủy hội thoại đang ở trạng thái pending.
    /// Luồng xử lý: xác thực requester, gửi command cancel, broadcast cập nhật realtime.
    /// </summary>
    /// <param name="id">Id hội thoại cần hủy.</param>
    /// <returns>Kết quả hủy hội thoại hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost("{id}/cancel")]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> CancelPending(string id)
    {
        if (!TryGetUserId(out var userId))
        {
            // Chặn thao tác hủy hội thoại khi thiếu danh tính người yêu cầu.
            return this.UnauthorizedProblem();
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
