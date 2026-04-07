using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Notification.Commands.MarkAsRead;
using TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

namespace TarotNow.Api.Controllers;

public partial class NotificationController
{
    [HttpPatch("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var command = new MarkNotificationReadCommand
        {
            NotificationId = id,
            UserId = userId
        };

        var success = await _mediator.Send(command);
        if (!success)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Notification not found",
                detail: "Notification not found.");
        }

        return Ok(new { message = "Notification marked as read." });
    }

    [HttpPatch("read-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MarkAllAsRead()
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var command = new MarkAllNotificationsReadCommand { UserId = userId };
        var modifiedAny = await _mediator.Send(command);

        if (!modifiedAny)
        {
            return NoContent();
        }

        return Ok(new { message = "All notifications marked as read." });
    }
}
