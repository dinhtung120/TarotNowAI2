using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
public sealed class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns paginated users for admin management.
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> ListUsers([FromQuery] TarotNow.Application.Features.Admin.Queries.ListUsers.ListUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user account from admin portal.
    /// </summary>
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] TarotNow.Application.Features.Admin.Commands.CreateUser.CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    /// <summary>
    /// Locks or unlocks a user account.
    /// </summary>
    [HttpPatch("users/lock")]
    public async Task<IActionResult> ToggleUserLock([FromBody] TarotNow.Application.Features.Admin.Commands.ToggleUserLock.ToggleUserLockCommand command)
    {
        var success = await _mediator.Send(command);
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update user lock state",
                detail: "Không thể cập nhật trạng thái khóa tài khoản.");
    }

    /// <summary>
    /// Updates user profile fields and status by user identifier.
    /// </summary>
    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] TarotNow.Application.Features.Admin.Commands.UpdateUser.UpdateUserCommand command)
    {
        command.UserId = id;
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            var headerKey = Request.Headers["X-Idempotency-Key"].ToString();
            command.IdempotencyKey = !string.IsNullOrWhiteSpace(headerKey) ? headerKey : Guid.NewGuid().ToString();
        }

        var result = await _mediator.Send(command);
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update user",
                detail: "Không thể cập nhật User.");
    }

    /// <summary>
    /// Adds balance to a user wallet with idempotency support.
    /// </summary>
    [HttpPost("users/add-balance")]
    public async Task<IActionResult> AddUserBalance([FromBody] TarotNow.Application.Features.Admin.Commands.AddUserBalance.AddUserBalanceCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            command.IdempotencyKey = Request.Headers["X-Idempotency-Key"].ToString();
        }

        var result = await _mediator.Send(command);
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot add user balance",
                detail: "Không thể cộng tiền cho người dùng này.");
    }
}
