/*
 * ===================================================================
 * FILE: ReportController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller cho phép user BÁO CÁO VI PHẠM.
 *   Khi phát hiện nội dung không phù hợp (spam, lừa đảo, quấy rối),
 *   user có thể báo cáo để admin xem xét và xử lý.
 *
 * CÁC LOẠI BÁO CÁO:
 *   - message: báo cáo một tin nhắn cụ thể
 *   - conversation: báo cáo toàn bộ cuộc trò chuyện
 *   - user: báo cáo một người dùng (reader hoặc user khác)
 *
 * LUỒNG XỬ LÝ:
 *   1. User tạo report → lưu vào MongoDB (collection: reports)
 *   2. Admin xem danh sách report trong admin panel (Phase tương lai)
 *   3. Admin xử lý: cảnh cáo, khóa tài khoản, xóa nội dung
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller báo cáo vi phạm.
/// User báo cáo tin nhắn, conversation, hoặc user vi phạm.
/// Admin xem xét + xử lý qua admin panel (Phase tương lai).
/// </summary>
[Route(ApiRoutes.Reports)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] // Phải đăng nhập mới được báo cáo (để biết ai báo cáo)
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/reports
    /// MỤC ĐÍCH: Tạo báo cáo vi phạm mới.
    ///
    /// DỮ LIỆU GỬI LÊN:
    ///   - targetType: loại đối tượng bị báo cáo ("message", "conversation", "user")
    ///   - targetId: ID của đối tượng bị báo cáo
    ///   - conversationRef: ID cuộc trò chuyện liên quan (tùy chọn)
    ///   - reason: lý do báo cáo (bắt buộc, tối thiểu 10 ký tự)
    ///
    /// THÔNG TIN TỰ ĐỘNG THÊM:
    ///   - ReporterId: lấy từ JWT (ai đang báo cáo)
    ///   - Timestamp: thời điểm báo cáo (tự động ở handler)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReportBody body)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var command = new CreateReportCommand
        {
            ReporterId = userId,                   // Người báo cáo (từ JWT)
            TargetType = body.TargetType,           // Loại: message/conversation/user
            TargetId = body.TargetId,               // ID đối tượng bị báo cáo
            ConversationRef = body.ConversationRef, // Conversation liên quan (tùy chọn)
            Reason = body.Reason                    // Lý do báo cáo
        };

        // Handler tạo document report trong MongoDB, trả về report với Id
        var result = await _mediator.Send(command);
        return Ok(new { success = true, reportId = result.Id });
    }
}
