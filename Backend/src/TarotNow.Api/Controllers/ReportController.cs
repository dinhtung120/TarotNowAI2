

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Reports)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] 
// API báo cáo vi phạm/sự cố.
// Luồng chính: xác thực người gửi báo cáo, map payload sang command và tạo bản ghi report.
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller báo cáo.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command tạo report.</param>
    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tạo báo cáo mới từ người dùng.
    /// Luồng xử lý: xác thực user, map body sang command, tạo report và trả report id.
    /// </summary>
    /// <param name="body">Payload thông tin báo cáo.</param>
    /// <returns>Kết quả success kèm id report mới tạo.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReportBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn tạo report khi không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Mapping đầy đủ để handler có đủ ngữ cảnh xử lý và lưu report.
        var command = new CreateReportCommand
        {
            ReporterId = userId,                   
            TargetType = body.TargetType,           
            TargetId = body.TargetId,               
            ConversationRef = body.ConversationRef, 
            Reason = body.Reason                    
        };

        var result = await _mediator.Send(command);
        return Ok(new { success = true, reportId = result.Id });
    }
}
