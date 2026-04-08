using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Notification.Queries.CountUnread;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;

namespace TarotNow.Api.Controllers;

public partial class NotificationController
{
    /// <summary>
    /// Lấy danh sách thông báo của người dùng theo phân trang.
    /// Luồng xử lý: xác thực user id, gửi query với bộ lọc isRead tùy chọn.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số thông báo mỗi trang.</param>
    /// <param name="isRead">Bộ lọc trạng thái đã đọc/chưa đọc.</param>
    /// <returns>Danh sách thông báo theo điều kiện truy vấn.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotificationListResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? isRead = null)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn thông báo khi không có danh tính người dùng hợp lệ.
            return this.UnauthorizedProblem();
        }

        var query = new GetNotificationsQuery
        {
            UserId = userId,
            Page = page,
            PageSize = pageSize,
            IsRead = isRead
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Lấy tổng số thông báo chưa đọc.
    /// </summary>
    /// <returns>Số lượng thông báo chưa đọc của người dùng hiện tại.</returns>
    [HttpGet("unread-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUnreadCount()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn unread count khi user chưa xác thực.
            return this.UnauthorizedProblem();
        }

        var count = await _mediator.Send(new CountUnreadQuery(userId));
        return Ok(new { count });
    }
}
