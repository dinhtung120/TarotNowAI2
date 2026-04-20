using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị nạp tiền.
// Luồng chính: liệt kê đơn nạp và xử lý duyệt/từ chối đơn từ admin.
public sealed class AdminDepositsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị nạp tiền.
    /// </summary>
    /// <param name="mediator">MediatR dùng để gọi tầng ứng dụng.</param>
    public AdminDepositsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách đơn nạp theo bộ lọc truy vấn.
    /// Luồng xử lý: nhận query từ client và chuyển thẳng cho handler để giữ đúng CQRS boundary.
    /// </summary>
    /// <param name="query">Bộ lọc/phân trang đơn nạp.</param>
    /// <returns>Danh sách đơn nạp theo điều kiện truy vấn.</returns>
    [HttpGet("deposits")]
    public async Task<IActionResult> ListDeposits([FromQuery] TarotNow.Application.Features.Admin.Queries.ListDeposits.ListDepositsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Xử lý một đơn nạp tiền trong hàng chờ.
    /// Luồng manual process đã bị vô hiệu hóa vì hệ thống chuyển sang webhook PayOS hoàn toàn tự động.
    /// </summary>
    /// <returns>410 Gone để buộc client dùng luồng webhook chính thống.</returns>
    [HttpPatch("deposits/process")]
    public IActionResult ProcessDeposit()
    {
        return Problem(
            statusCode: StatusCodes.Status410Gone,
            title: "Manual deposit processing is disabled",
            detail: "Deposit orders are processed automatically via PayOS webhooks.");
    }
}
