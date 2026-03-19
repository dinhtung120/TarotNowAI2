/*
 * ===================================================================
 * FILE: HistoryController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý LỊCH SỬ CÁC PHIÊN ĐỌC BÀI TAROT.
 *   Cho phép:
 *   1. User xem lại các phiên đọc bài đã hoàn thành
 *   2. User xem chi tiết một phiên (bài nào, ý nghĩa gì, chat follow-up)
 *   3. Admin xem tất cả phiên đọc bài của mọi user (quản trị)
 *
 * DỮ LIỆU LỊCH SỬ BAO GỒM:
 *   - Thời gian đọc bài
 *   - Kiểu trải bài (spread type): 1 lá, 3 lá, Celtic Cross, v.v.
 *   - Các lá bài đã rút
 *   - Nội dung phân tích AI
 *   - Lịch sử chat follow-up với AI (nếu có)
 * ===================================================================
 */

using MediatR;                 // MediatR trung gian
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền
using Microsoft.AspNetCore.Mvc; // API controller

// Import các Query cho lịch sử đọc bài
using TarotNow.Application.Features.History.Queries.GetReadingDetail;  // Chi tiết 1 phiên
using TarotNow.Application.Features.History.Queries.GetReadingHistory; // Danh sách phiên
using TarotNow.Application.Features.History.Queries.GetAllReadings;    // Tất cả phiên (admin)

namespace TarotNow.Api.Controllers;

/*
 * [Authorize]: Tất cả endpoint yêu cầu đăng nhập.
 *   Lịch sử đọc bài là dữ liệu CÁ NHÂN → không cho phép xem nếu chưa đăng nhập.
 */
[ApiController]
[Route("api/v1/[controller]")]
[Authorize] // Yêu cầu đăng nhập (Header: "Authorization: Bearer <token>")
public class HistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public HistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/History/sessions?page=1&pageSize=10
    /// MỤC ĐÍCH: Lấy danh sách lịch sử bốc bài Tarot của user hiện tại.
    ///
    /// PHÂN TRANG:
    ///   - page: trang hiện tại (mặc định 1)
    ///   - pageSize: số phiên mỗi trang (mặc định 10, tối đa 50)
    ///
    /// GUARD CLAUSES (kiểm tra đầu vào an toàn):
    ///   - page > 0 ? page : 1       → nếu page ≤ 0, dùng trang 1
    ///   - pageSize > 0 and ≤ 50     → giới hạn 1-50, mặc định 10
    ///   Giới hạn pageSize tối đa 50 để tránh client yêu cầu tải quá nhiều dữ liệu
    ///   → server không bị quá tải.
    /// </summary>
    [HttpGet("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingHistoryResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSessions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // Trích xuất UserId từ JWT Access Token
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var query = new GetReadingHistoryQuery
        {
            UserId = userId,
            /*
             * Guard clause cho page:
             * "page > 0 ? page : 1" nghĩa là:
             *   - Nếu page > 0 → dùng giá trị page
             *   - Nếu page ≤ 0 → dùng 1 (trang đầu)
             * Tránh lỗi khi client gửi page=0 hoặc page=-1
             */
            Page = page > 0 ? page : 1,

            /*
             * Guard clause cho pageSize:
             * "pageSize is > 0 and <= 50" là C# pattern matching:
             *   - pageSize phải lớn hơn 0 VÀ nhỏ hơn hoặc bằng 50
             *   - Nếu không thỏa mãn → dùng mặc định 10
             * Ngăn chặn client yêu cầu pageSize=10000 gây quá tải
             */
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/History/sessions/{id}
    /// MỤC ĐÍCH: Xem chi tiết một phiên bốc bài cụ thể.
    ///
    /// [FromRoute]: Tham số "id" được lấy từ URL path (phần {id}).
    ///   Ví dụ: GET /api/v1/History/sessions/abc-123
    ///   → id = "abc-123"
    ///
    /// TRẢ VỀ: Chi tiết phiên bao gồm:
    ///   - Các lá bài đã rút (vị trí, tên bài, hướng)
    ///   - Kiểu trải bài (spread type)
    ///   - Nội dung phân tích AI
    ///   - Lịch sử chat follow-up với AI
    ///
    /// BẢO MẬT: Handler kiểm tra sessionId thuộc về userId.
    ///   User A không thể xem phiên đọc bài của User B.
    /// </summary>
    [HttpGet("sessions/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingDetailResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionDetails([FromRoute] string id)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var query = new GetReadingDetailQuery
        {
            UserId = userId,   // User hiện tại (từ JWT)
            SessionId = id     // Session cần xem (từ URL)
        };

        var result = await _mediator.Send(query);

        // Nếu không tìm thấy → 404 Not Found (session không tồn tại hoặc không thuộc về user)
        if (result == null) return NotFound(new { message = "Reading session not found." });
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/History/admin/all-sessions
    /// MỤC ĐÍCH: Admin xem TẤT CẢ phiên đọc bài của mọi user.
    ///
    /// [Authorize(Roles = "admin")]: Chỉ admin mới truy cập được.
    ///
    /// BỘ LỌC (optional filters):
    ///   - username: lọc theo tên user
    ///   - spreadType: lọc theo kiểu trải bài (1 lá, 3 lá, v.v.)
    ///   - startDate, endDate: lọc theo khoảng thời gian
    ///   Tất cả đều nullable (?) = không bắt buộc.
    ///   Nếu không truyền → lấy tất cả.
    /// </summary>
    [HttpGet("admin/all-sessions")]
    [Authorize(Roles = "admin")] // Chỉ admin
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllReadingsResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)] // User thường gọi → 403 Forbidden
    public async Task<IActionResult> GetAllSessions(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? username = null,    // Lọc theo username (tùy chọn)
        [FromQuery] string? spreadType = null,  // Lọc theo kiểu trải bài (tùy chọn)
        [FromQuery] DateTime? startDate = null, // Lọc từ ngày (tùy chọn)
        [FromQuery] DateTime? endDate = null)   // Lọc đến ngày (tùy chọn)
    {
        var query = new GetAllReadingsQuery
        {
            Page = page > 0 ? page : 1,
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10,
            Username = username,       // null = không lọc
            SpreadType = spreadType,   // null = không lọc
            StartDate = startDate,     // null = không giới hạn ngày bắt đầu
            EndDate = endDate          // null = không giới hạn ngày kết thúc
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
