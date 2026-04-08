using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị hồ sơ đăng ký reader.
// Luồng chính: liệt kê yêu cầu và duyệt/từ chối đơn đăng ký.
public sealed class AdminReaderRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị yêu cầu reader.
    /// </summary>
    /// <param name="mediator">MediatR điều phối nghiệp vụ.</param>
    public AdminReaderRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách yêu cầu đăng ký reader theo query filter.
    /// </summary>
    /// <param name="query">Bộ lọc và phân trang yêu cầu reader.</param>
    /// <returns>Danh sách yêu cầu reader theo điều kiện truy vấn.</returns>
    [HttpGet("reader-requests")]
    public async Task<IActionResult> ListReaderRequests([FromQuery] TarotNow.Application.Features.Admin.Queries.ListReaderRequests.ListReaderRequestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Xử lý một yêu cầu đăng ký reader.
    /// Luồng xử lý: xác thực admin, ánh xạ body sang command, trả nhánh success/failure.
    /// </summary>
    /// <param name="body">Payload xử lý yêu cầu reader.</param>
    /// <returns>Kết quả thành công hoặc lỗi nghiệp vụ khi không thể xử lý.</returns>
    [HttpPatch("reader-requests/process")]
    public async Task<IActionResult> ProcessReaderRequest([FromBody] ProcessReaderRequestBody body)
    {
        if (!User.TryGetUserId(out var adminId))
        {
            // Chặn sớm request không có danh tính admin để bảo toàn audit trail.
            return this.UnauthorizedProblem();
        }

        // Mapping rõ ràng sang command để handler tập trung xử lý rule approve/reject.
        var command = new TarotNow.Application.Features.Admin.Commands.ApproveReader.ApproveReaderCommand
        {
            RequestId = body.RequestId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            AdminId = adminId
        };

        var result = await _mediator.Send(command);
        // Rẽ nhánh phản hồi để dashboard hiển thị chính xác trạng thái xử lý cuối cùng.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process reader request",
                detail: "Không thể xử lý đơn xin Reader.");
    }
}
