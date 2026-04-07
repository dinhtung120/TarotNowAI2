using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Notification.Queries.CountUnread;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;

namespace TarotNow.Api.Controllers;

public partial class NotificationController
{
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

    [HttpGet("unread-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUnreadCount()
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var count = await _mediator.Send(new CountUnreadQuery(userId));
        return Ok(new { count });
    }
}
