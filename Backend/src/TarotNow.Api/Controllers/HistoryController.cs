

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Extensions;


using TarotNow.Application.Features.History.Queries.GetReadingDetail;  
using TarotNow.Application.Features.History.Queries.GetReadingHistory; 
using TarotNow.Application.Features.History.Queries.GetAllReadings;    

namespace TarotNow.Api.Controllers;


[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Controller)]
[Authorize] 
[EnableRateLimiting("auth-session")]
// API lịch sử reading session.
// Luồng chính: lấy danh sách phiên, chi tiết phiên và endpoint admin xem toàn bộ phiên.
public class HistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller lịch sử.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query lịch sử.</param>
    public HistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách reading session của người dùng hiện tại.
    /// Luồng xử lý: xác thực user id, chuẩn hóa phân trang, gửi query lấy lịch sử.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số bản ghi mỗi trang.</param>
    /// <param name="spreadType">Lọc theo loại trải bài (tùy chọn).</param>
    /// <param name="date">Lọc theo ngày trải bài (tùy chọn).</param>
    /// <returns>Dữ liệu lịch sử reading session.</returns>
    [HttpGet("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingHistoryResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSessions(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? spreadType = null,
        [FromQuery] DateTime? date = null)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy cập lịch sử khi claim định danh không hợp lệ.
            return this.UnauthorizedProblem();
        }

        var query = new GetReadingHistoryQuery
        {
            UserId = userId,
            // Chuẩn hóa phân trang để tránh giá trị âm/0 gây lỗi truy vấn.
            Page = page > 0 ? page : 1,

            // Giới hạn page size để bảo vệ hiệu năng endpoint.
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10,
            
            SpreadType = spreadType,
            Date = date
        };

        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);
        return Ok(result);
    }

    /// <summary>
    /// Lấy chi tiết một reading session của người dùng hiện tại.
    /// Luồng xử lý: xác thực user id, gửi query chi tiết, rẽ nhánh 404 khi session không tồn tại.
    /// </summary>
    /// <param name="id">Id session cần lấy chi tiết.</param>
    /// <returns>Dữ liệu chi tiết session hoặc lỗi 404/401 tương ứng.</returns>
    [HttpGet("sessions/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingDetailResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionDetails([FromRoute] string id)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn chi tiết khi user id không hợp lệ.
            return this.UnauthorizedProblem();
        }

        var query = new GetReadingDetailQuery
        {
            UserId = userId,   
            SessionId = id     
        };

        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);

        if (result == null)
        {
            // Trả 404 rõ ràng để client phân biệt session không tồn tại với lỗi hệ thống.
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Reading session not found",
                detail: "Reading session not found.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Endpoint admin lấy toàn bộ reading session trên hệ thống.
    /// Luồng xử lý: chuẩn hóa phân trang + filter tùy chọn, gửi query tổng hợp cho dashboard admin.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số bản ghi mỗi trang.</param>
    /// <param name="username">Bộ lọc username tùy chọn.</param>
    /// <param name="spreadType">Bộ lọc loại trải bài tùy chọn.</param>
    /// <param name="startDate">Mốc ngày bắt đầu lọc tùy chọn.</param>
    /// <param name="endDate">Mốc ngày kết thúc lọc tùy chọn.</param>
    /// <returns>Dữ liệu tổng hợp reading session cho admin.</returns>
    [HttpGet("admin/all-sessions")]
    [Authorize(Roles = "admin")] 
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllReadingsResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)] 
    public async Task<IActionResult> GetAllSessions(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? username = null,    
        [FromQuery] string? spreadType = null,  
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)   
    {
        var query = new GetAllReadingsQuery
        {
            Page = page > 0 ? page : 1,
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10,
            Username = username,       
            SpreadType = spreadType,   
            StartDate = startDate,     
            EndDate = endDate          
        };

        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);
        return Ok(result);
    }
}
