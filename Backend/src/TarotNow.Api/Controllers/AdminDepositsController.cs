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
    /// Luồng xử lý: dispatch command và rẽ nhánh phản hồi theo kết quả nghiệp vụ.
    /// </summary>
    /// <param name="command">Command xử lý đơn nạp từ admin.</param>
    /// <returns>Kết quả thành công hoặc lỗi nghiệp vụ khi không thể xử lý đơn.</returns>
    [HttpPatch("deposits/process")]
    public async Task<IActionResult> ProcessDeposit([FromBody] TarotNow.Application.Features.Admin.Commands.ProcessDeposit.ProcessDepositCommand command)
    {
        var result = await _mediator.Send(command);
        // Rẽ nhánh kết quả để chuẩn hóa response lỗi nghiệp vụ cho dashboard admin.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process deposit order",
                detail: "Không thể xử lý đơn nạp tiền này.");
    }
}
