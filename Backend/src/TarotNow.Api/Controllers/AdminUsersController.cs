using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị người dùng.
// Luồng chính: liệt kê, tạo, khóa/mở khóa, cập nhật thông tin và cộng số dư cho user.
public sealed class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị người dùng.
    /// </summary>
    /// <param name="mediator">MediatR dùng để dispatch command/query.</param>
    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách người dùng theo bộ lọc quản trị.
    /// </summary>
    /// <param name="query">Bộ lọc, sắp xếp và phân trang user.</param>
    /// <returns>Danh sách user theo query.</returns>
    [HttpGet("users")]
    public async Task<IActionResult> ListUsers([FromQuery] TarotNow.Application.Features.Admin.Queries.ListUsers.ListUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới một tài khoản người dùng từ màn hình admin.
    /// </summary>
    /// <param name="command">Command tạo user.</param>
    /// <returns>Id người dùng vừa được tạo.</returns>
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] TarotNow.Application.Features.Admin.Commands.CreateUser.CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    /// <summary>
    /// Khóa hoặc mở khóa tài khoản người dùng.
    /// Luồng xử lý: dispatch command và trả nhánh lỗi nghiệp vụ nếu cập nhật thất bại.
    /// </summary>
    /// <param name="command">Command thay đổi trạng thái khóa user.</param>
    /// <returns>Kết quả thành công hoặc lỗi nghiệp vụ.</returns>
    [HttpPatch("users/lock")]
    public async Task<IActionResult> ToggleUserLock([FromBody] TarotNow.Application.Features.Admin.Commands.ToggleUserLock.ToggleUserLockCommand command)
    {
        var success = await _mediator.Send(command);
        // Tách response lỗi riêng để dashboard có thông điệp rõ ràng khi thao tác thất bại.
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update user lock state",
                detail: "Không thể cập nhật trạng thái khóa tài khoản.");
    }

    /// <summary>
    /// Cập nhật thông tin người dùng theo id.
    /// Luồng xử lý: gắn id route vào command, bảo đảm idempotency key, dispatch cập nhật.
    /// </summary>
    /// <param name="id">Id người dùng cần cập nhật.</param>
    /// <param name="command">Command cập nhật thông tin user.</param>
    /// <returns>Kết quả thành công hoặc lỗi nghiệp vụ khi cập nhật thất bại.</returns>
    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] TarotNow.Application.Features.Admin.Commands.UpdateUser.UpdateUserCommand command)
    {
        // Đồng bộ id từ route vào command để chặn sai lệch giữa URL và payload.
        command.UserId = id;
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            var headerKey = Request.GetIdempotencyKeyOrEmpty();
            // Ưu tiên idempotency key từ header để giữ semantics retry phía client.
            command.IdempotencyKey = !string.IsNullOrWhiteSpace(headerKey) ? headerKey : string.Empty;
        }

        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Missing idempotency key",
                detail: "Idempotency-Key is required.");
        }

        var result = await _mediator.Send(command);
        // Rẽ nhánh chuẩn hóa kết quả nhằm giữ hợp đồng phản hồi nhất quán cho client admin.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update user",
                detail: "Không thể cập nhật User.");
    }

    /// <summary>
    /// Cộng số dư cho người dùng từ kênh quản trị.
    /// Luồng xử lý: bảo đảm idempotency key rồi dispatch command cộng tiền.
    /// </summary>
    /// <param name="command">Command cộng số dư cho user.</param>
    /// <returns>Kết quả thành công hoặc lỗi nghiệp vụ.</returns>
    [HttpPost("users/add-balance")]
    public async Task<IActionResult> AddUserBalance([FromBody] TarotNow.Application.Features.Admin.Commands.AddUserBalance.AddUserBalanceCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            // Đọc idempotency key từ header để giảm rủi ro cộng tiền trùng khi retry request.
            command.IdempotencyKey = Request.GetIdempotencyKeyOrEmpty();
        }

        var result = await _mediator.Send(command);
        // Tách nhánh lỗi để cảnh báo rõ thao tác cộng tiền không thành công.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot add user balance",
                detail: "Không thể cộng tiền cho người dùng này.");
    }
}
