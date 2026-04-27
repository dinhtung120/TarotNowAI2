using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Admin.Commands.ResolveDispute;
using TarotNow.Application.Features.Admin.Queries.ListDisputes;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.AdminDisputes)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị tranh chấp.
// Luồng chính: liệt kê item tranh chấp và ghi nhận quyết định resolve từ admin.
public sealed class AdminDisputesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị tranh chấp.
    /// </summary>
    /// <param name="mediator">MediatR dùng để dispatch query/command.</param>
    public AdminDisputesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách tranh chấp theo phân trang.
    /// Luồng xử lý: đóng gói tham số phân trang vào query rồi chuyển cho tầng ứng dụng.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số phần tử mỗi trang.</param>
    /// <returns>Danh sách tranh chấp đã phân trang.</returns>
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new ListDisputesQuery { Page = page, PageSize = pageSize });
        return Ok(result);
    }

    /// <summary>
    /// Resolve một item tranh chấp.
    /// Luồng xử lý: xác thực admin id, dựng command từ payload, thực thi và trả kết quả.
    /// </summary>
    /// <param name="id">Id item tranh chấp.</param>
    /// <param name="body">Thông tin quyết định resolve.</param>
    /// <returns>Kết quả thành công hoặc unauthorized khi không có admin id hợp lệ.</returns>
    [HttpPost("{id:guid}/resolve")]
    public async Task<IActionResult> Resolve(Guid id, [FromBody] AdminResolveDisputeBody body)
    {
        if (!User.TryGetUserId(out var adminId))
        {
            // Chặn sớm khi không trích xuất được danh tính admin để tránh thao tác không truy vết.
            return this.UnauthorizedProblem();
        }

        // Ánh xạ đầy đủ payload sang command để handler xử lý đúng rule nghiệp vụ settlement.
        await _mediator.SendWithRequestCancellation(HttpContext, new ResolveDisputeCommand
        {
            ItemId = id,
            AdminId = adminId,
            Action = body.Action,
            SplitPercentToReader = body.SplitPercentToReader,
            AdminNote = body.AdminNote
        });

        return Ok(new { success = true, itemId = id, action = body.Action, splitPercentToReader = body.SplitPercentToReader });
    }
}
