

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Threading.Tasks;


using TarotNow.Api.Contracts; 
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Legal.Commands.RecordConsent;
using TarotNow.Application.Features.Legal.Queries.CheckConsent;

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Legal)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
// API pháp lý và ghi nhận consent người dùng.
// Luồng chính: kiểm tra trạng thái consent và ghi nhận consent mới kèm metadata truy vết.
public class LegalController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Khởi tạo controller legal.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query/command consent.</param>
    public LegalController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Kiểm tra trạng thái đồng ý điều khoản của người dùng.
    /// Luồng xử lý: xác thực user id, gửi query theo document/version tùy chọn.
    /// </summary>
    /// <param name="documentType">Loại tài liệu pháp lý cần kiểm tra.</param>
    /// <param name="version">Phiên bản tài liệu cần kiểm tra.</param>
    /// <returns>Trạng thái consent hiện tại của người dùng.</returns>
    [HttpGet("consent-status")]
    [Authorize]
    public async Task<IActionResult> CheckConsentStatus([FromQuery] string? documentType, [FromQuery] string? version)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn consent khi không có danh tính hợp lệ.
            return this.UnauthorizedProblem();
        }
 
        var query = new CheckConsentQuery 
        { 
            UserId = userId,
            DocumentType = documentType, 
            Version = version            
        };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Ghi nhận người dùng đã đồng ý một tài liệu pháp lý.
    /// Luồng xử lý: xác thực user, lấy metadata request (IP/User-Agent), gửi command ghi nhận consent.
    /// </summary>
    /// <param name="request">Payload loại tài liệu và phiên bản đã đồng ý.</param>
    /// <returns>Kết quả success khi ghi nhận consent thành công.</returns>
    [HttpPost("consent")]
    [Authorize]
    public async Task<IActionResult> RecordConsent([FromBody] RecordConsentRequest request)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Bắt buộc xác thực để consent luôn gắn với chủ thể cụ thể.
            return this.UnauthorizedProblem();
        }

        // Thu thập IP để phục vụ audit pháp lý và điều tra khi cần.
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        // Thu thập User-Agent để bổ sung ngữ cảnh thiết bị khi ghi nhận consent.
        var userAgent = Request.Headers["User-Agent"].ToString();

        var command = new RecordConsentCommand
        {
            UserId = userId,
            DocumentType = request.DocumentType, 
            Version = request.Version,           
            IpAddress = ipAddress,               
            UserAgent = userAgent                
        };

        await Mediator.Send(command);
        return Ok(new { success = true });
    }
}
