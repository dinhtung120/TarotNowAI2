using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Reader.Queries.GetReaderProfile;
using TarotNow.Application.Features.Reader.Queries.ListReaders;

namespace TarotNow.Api.Controllers;

public partial class ReaderController
{
    /// <summary>
    /// Lấy hồ sơ reader theo user id.
    /// Luồng xử lý: gửi query profile, rẽ nhánh 404 nếu không tồn tại, đồng bộ trạng thái presence trước khi trả.
    /// </summary>
    /// <param name="userId">Id user của reader cần xem hồ sơ.</param>
    /// <returns>Hồ sơ reader hoặc lỗi 404 khi không tìm thấy.</returns>
    [HttpGet("profile/{userId}")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var profile = await _mediator.SendWithRequestCancellation(HttpContext, new GetReaderProfileQuery { UserId = userId });
        if (profile == null)
        {
            // Trả 404 rõ ràng để client xử lý trường hợp reader không tồn tại.
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Reader profile not found",
                detail: "Không tìm thấy hồ sơ Reader.");
        }

        ApplyPresenceStatus(profile);
        return Ok(profile);
    }

    /// <summary>
    /// Liệt kê danh sách reader theo query filter.
    /// Luồng xử lý: lấy danh sách từ query và đồng bộ presence status cho từng reader trước khi trả.
    /// </summary>
    /// <param name="query">Bộ lọc/tìm kiếm/phân trang reader.</param>
    /// <returns>Danh sách reader đã đồng bộ trạng thái online.</returns>
    [HttpGet(ApiRoutes.ReadersAbsolute)]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> ListReaders([FromQuery] ListReadersQuery query)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);
        foreach (var reader in result.Readers)
        {
            // Đồng bộ realtime status từng reader để directory hiển thị chính xác hơn.
            ApplyPresenceStatus(reader);
        }

        return Ok(result);
    }
}
