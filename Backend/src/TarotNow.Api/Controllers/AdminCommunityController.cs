

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Community.Commands.ResolvePostReport;
using TarotNow.Application.Features.Community.Queries.GetModerationQueue;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.AdminCommunity)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize(Roles = ApiRoleConstants.Admin)] 
[EnableRateLimiting("auth-session")]
// API quản trị cộng đồng dành cho moderator/admin.
// Luồng chính: lấy hàng chờ moderation và ghi nhận quyết định xử lý báo cáo.
public class AdminCommunityController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller với mediator để điều phối query/command đến tầng ứng dụng.
    /// </summary>
    /// <param name="mediator">MediatR dùng để dispatch nghiệp vụ.</param>
    public AdminCommunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách báo cáo cần moderation theo phân trang và bộ lọc trạng thái.
    /// Luồng xử lý: dựng query từ input, gọi nghiệp vụ, trả dữ liệu kèm metadata phân trang.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số phần tử mỗi trang.</param>
    /// <param name="statusFilter">Bộ lọc trạng thái tùy chọn.</param>
    /// <returns>Kết quả moderation queue và metadata phân trang.</returns>
    [HttpGet("reports")]
    public async Task<IActionResult> GetModerationQueue(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? statusFilter = null)
    {
        // Ánh xạ toàn bộ tham số đầu vào vào query để giữ rõ ràng boundary API -> Application.
        var query = new GetModerationQueueQuery
        {
            Page = page,
            PageSize = pageSize,
            StatusFilter = statusFilter
        };

        var (items, total) = await _mediator.SendWithRequestCancellation(HttpContext, query);
        // Trả metadata phân trang để client đồng bộ UI và điều hướng trang tiếp theo.
        return Ok(new
        {
            success = true,
            data = items,
            metadata = new { totalCount = total, page, pageSize }
        });
    }

    /// <summary>
    /// Ghi nhận quyết định xử lý một báo cáo cộng đồng.
    /// Luồng xử lý: lấy admin hiện tại, dựng command từ body, dispatch đến tầng ứng dụng.
    /// </summary>
    /// <param name="id">Id báo cáo cần xử lý.</param>
    /// <param name="body">Payload chứa kết quả xử lý và ghi chú admin.</param>
    /// <returns>Kết quả thành công khi xử lý hoàn tất.</returns>
    [HttpPut("reports/{id}/resolve")]
    public async Task<IActionResult> ResolveReport(string id, [FromBody] ResolveReportBody body)
    {
        // Rule phân quyền bổ sung: mọi quyết định moderation phải truy vết được admin thực hiện.
        var adminId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();

        // Dựng command rõ ràng để tách trách nhiệm mapping khỏi handler nghiệp vụ.
        var command = new ResolvePostReportCommand
        {
            ReportId = id,
            AdminId = adminId,
            Result = body.Result,
            AdminNote = body.AdminNote
        };

        await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(new { success = true });
    }
}
