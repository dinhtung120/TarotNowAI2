/*
 * ===================================================================
 * FILE: LegalController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý ĐỒNG Ý PHÁP LÝ (Legal Consent) của người dùng.
 *   
 * TẠI SAO CẦN?
 *   Theo luật bảo vệ dữ liệu cá nhân (GDPR ở EU, PDPA ở Việt Nam):
 *   - Ứng dụng PHẢI ghi nhận sự đồng ý của user với Điều khoản sử dụng
 *   - Phải lưu lại: ai đồng ý, lúc nào, phiên bản nào, từ IP nào
 *   - Khi cập nhật điều khoản → user phải đồng ý lại phiên bản mới
 *   
 * CÁC ENDPOINT:
 *   GET  /consent-status - Kiểm tra user đã đồng ý chưa
 *   POST /consent        - Ghi nhận sự đồng ý mới
 * ===================================================================
 */

using MediatR;                 // MediatR trung gian
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền
using Microsoft.AspNetCore.Mvc; // API controller
using System;
using System.Threading.Tasks;

// Import DTO và Command/Query
using TarotNow.Api.Contracts; // RecordConsentRequest DTO
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Legal.Commands.RecordConsent;
using TarotNow.Application.Features.Legal.Queries.CheckConsent;

namespace TarotNow.Api.Controllers;

/*
 * KHÔNG có [Authorize] ở cấp class vì trong tương lai có thể cần
 * endpoint public (ví dụ: xem nội dung Điều khoản mà không cần đăng nhập).
 * Các endpoint cần auth đánh dấu [Authorize] riêng.
 */
[Route(ApiRoutes.Legal)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
public class LegalController : ControllerBase
{
    private readonly IMediator Mediator;

    public LegalController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/legal/consent-status?documentType=tos&amp;version=1.0
    /// MỤC ĐÍCH: Kiểm tra user đã đồng ý với văn bản pháp lý cụ thể chưa.
    ///
    /// CÁC TRƯỜNG HỢP SỬ DỤNG:
    ///   1. Khi user đăng nhập → client kiểm tra consent mới nhất.
    ///      Nếu chưa đồng ý TOS mới nhất → hiển thị popup yêu cầu đồng ý.
    ///   2. Trước khi thực hiện hành động nhạy cảm (ví dụ: nạp tiền)
    ///      → kiểm tra user đã đồng ý privacy policy chưa.
    ///
    /// THAM SỐ (tùy chọn):
    ///   - documentType: loại văn bản ("tos", "privacy", "ai-disclaimer")
    ///   - version: phiên bản cụ thể ("1.0", "2.3")
    ///   Nếu không truyền → trả về trạng thái tổng quan tất cả các văn bản.
    /// </summary>
    [HttpGet("consent-status")]
    [Authorize]
    public async Task<IActionResult> CheckConsentStatus([FromQuery] string? documentType, [FromQuery] string? version)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
 
        var query = new CheckConsentQuery 
        { 
            UserId = userId,
            DocumentType = documentType, // null = kiểm tra tất cả
            Version = version            // null = kiểm tra phiên bản mới nhất
        };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/legal/consent
    /// MỤC ĐÍCH: Ghi nhận sự đồng ý pháp lý của user.
    ///
    /// DỮ LIỆU GHI LẠI (để tuân thủ luật):
    ///   - UserId: ai đồng ý
    ///   - DocumentType: đồng ý văn bản nào (tos, privacy, ai-disclaimer)
    ///   - Version: phiên bản bao nhiêu
    ///   - IpAddress: đồng ý từ IP nào (vị trí mạng)
    ///   - UserAgent: đồng ý từ thiết bị/trình duyệt nào
    ///   - Timestamp: đồng ý lúc nào (tự động ở server)
    ///
    /// TẠI SAO LƯU IP VÀ USER-AGENT?
    ///   Đây là yêu cầu của GDPR: phải chứng minh được consent là genuine
    ///   (thật sự đến từ user, không bị giả mạo). IP và User-Agent giúp audit.
    /// </summary>
    [HttpPost("consent")]
    [Authorize]
    public async Task<IActionResult> RecordConsent([FromBody] RecordConsentRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        /*
         * Lấy IP address từ kết nối HTTP:
         * RemoteIpAddress: IP của client gửi request.
         * "?." (null-conditional): nếu không lấy được IP → dùng "unknown".
         */
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        /*
         * Lấy User-Agent từ HTTP header:
         * User-Agent chứa thông tin trình duyệt/thiết bị.
         * Ví dụ: "Mozilla/5.0 (iPhone; CPU iPhone OS 16_0) Safari/604.1"
         * → biết user dùng iPhone, iOS 16, Safari.
         */
        var userAgent = Request.Headers["User-Agent"].ToString();

        var command = new RecordConsentCommand
        {
            UserId = userId,
            DocumentType = request.DocumentType, // Loại văn bản (từ body)
            Version = request.Version,           // Phiên bản (từ body)
            IpAddress = ipAddress,               // IP client (từ connection)
            UserAgent = userAgent                // Thiết bị/trình duyệt (từ header)
        };

        await Mediator.Send(command);
        return Ok(new { success = true });
    }
}
