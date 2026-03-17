using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Application.Features.Chat.Commands.CreateReport;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller báo cáo vi phạm.
///
/// User báo cáo tin nhắn, conversation, hoặc user vi phạm.
/// Admin xem xét + xử lý qua admin panel (Phase tương lai).
/// </summary>
[Route("api/v1/reports")]
[ApiController]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tạo báo cáo vi phạm.
    /// Body: { targetType, targetId, conversationRef?, reason }
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReportBody body)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

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

/// <summary>Body cho POST /reports.</summary>
public class CreateReportBody
{
    /// <summary>Loại: message | conversation | user.</summary>
    public string TargetType { get; set; } = string.Empty;

    /// <summary>ID đối tượng bị báo cáo.</summary>
    public string TargetId { get; set; } = string.Empty;

    /// <summary>ObjectId conversation liên quan (tùy chọn).</summary>
    public string? ConversationRef { get; set; }

    /// <summary>Lý do báo cáo (tối thiểu 10 ký tự).</summary>
    public string Reason { get; set; } = string.Empty;
}
