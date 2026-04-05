/*
 * ===================================================================
 * FILE: AdminCommunityController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH:
 *   Controller dành cho Tòa Án Tối Cao (Admin) để duyệt các đơn tố cáo (Reports)
 *   về các bài viết trong hệ thống.
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Community.Commands.ResolvePostReport;
using TarotNow.Application.Features.Community.Queries.GetModerationQueue;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.AdminCommunity)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize(Roles = ApiRoleConstants.Admin)] // Tường lửa: Chỉ Admin mới được bước vào đây
public class AdminCommunityController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminCommunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy hàng đợi các báo cáo community post để admin kiểm duyệt.
    /// </summary>
    [HttpGet("reports")]
    public async Task<IActionResult> GetModerationQueue(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? statusFilter = null)
    {
        var query = new GetModerationQueueQuery
        {
            Page = page,
            PageSize = pageSize,
            StatusFilter = statusFilter
        };

        var (items, total) = await _mediator.Send(query);
        return Ok(new
        {
            success = true,
            data = items,
            metadata = new { totalCount = total, page, pageSize }
        });
    }

    /// <summary>
    /// Xử lý một báo cáo community post theo quyết định của admin.
    /// </summary>
    [HttpPut("reports/{id}/resolve")]
    public async Task<IActionResult> ResolveReport(string id, [FromBody] ResolveReportBody body)
    {
        var adminId = User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
        var command = new ResolvePostReportCommand
        {
            ReportId = id,
            AdminId = adminId,
            Result = body.Result,
            AdminNote = body.AdminNote
        };

        await _mediator.Send(command);
        return Ok(new { success = true });
    }
}
