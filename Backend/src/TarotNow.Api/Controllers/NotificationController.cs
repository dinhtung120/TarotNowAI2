

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using TarotNow.Application.Features.Notification.Commands.MarkAsRead;
using TarotNow.Application.Features.Notification.Queries.CountUnread;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
public class NotificationController : ControllerBase
{
        private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotificationListResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? isRead = null)
    {
        
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        
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
            return Unauthorized();

        
        var count = await _mediator.Send(new CountUnreadQuery(userId));

        
        return Ok(new { count });
    }

        [HttpPatch("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        
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
            return Unauthorized();

        var command = new TarotNow.Application.Features.Notification.Commands.MarkAllAsRead.MarkAllNotificationsReadCommand
        {
            UserId = userId
        };

        var modifiedAny = await _mediator.Send(command);

        if (!modifiedAny)
        {
            return NoContent(); 
        }

        return Ok(new { message = "All notifications marked as read." });
    }
}
