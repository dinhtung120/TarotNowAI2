using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Notification.Commands.MarkAsRead;
using TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

namespace TarotNow.Api.Controllers;

public partial class NotificationController
{
    /// <summary>
    /// Đánh dấu một thông báo là đã đọc.
    /// Luồng xử lý: xác thực user id, gửi command mark-read, trả 404 nếu thông báo không tồn tại.
    /// </summary>
    /// <param name="id">Id thông báo cần đánh dấu.</param>
    /// <returns>Kết quả cập nhật trạng thái đọc của thông báo.</returns>
    [HttpPatch("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn cập nhật trạng thái thông báo khi không có danh tính hợp lệ.
            return this.UnauthorizedProblem();
        }

        var command = new MarkNotificationReadCommand
        {
            NotificationId = id,
            UserId = userId
        };

        var success = await _mediator.SendWithRequestCancellation(HttpContext, command);
        if (!success)
        {
            // Trả 404 để client biết id thông báo không còn tồn tại hoặc không thuộc user hiện tại.
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Notification not found",
                detail: "Notification not found.");
        }

        return Ok(new { message = "Notification marked as read." });
    }

    /// <summary>
    /// Đánh dấu toàn bộ thông báo của user là đã đọc.
    /// Luồng xử lý: xác thực user, gửi command mark-all, trả 204 nếu không có bản ghi thay đổi.
    /// </summary>
    /// <returns>Kết quả cập nhật hàng loạt trạng thái đã đọc.</returns>
    [HttpPatch("read-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MarkAllAsRead()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn thao tác mark-all khi request không có user id.
            return this.UnauthorizedProblem();
        }

        var command = new MarkAllNotificationsReadCommand { UserId = userId };
        var modifiedAny = await _mediator.SendWithRequestCancellation(HttpContext, command);

        if (!modifiedAny)
        {
            // Không có thay đổi thực tế thì trả 204 để biểu thị idempotent no-op.
            return NoContent();
        }

        return Ok(new { message = "All notifications marked as read." });
    }
}
