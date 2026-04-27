using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.Tasks;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;
using TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.CheckIn)]
[Authorize]
[EnableRateLimiting("auth-session")]
// API điểm danh và quản lý chuỗi streak hằng ngày.
// Luồng chính: check-in ngày mới, lấy trạng thái streak, mua lượt đóng băng streak.
public class CheckInController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller check-in.
    /// </summary>
    /// <param name="mediator">MediatR dùng để dispatch command/query streak.</param>
    public CheckInController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Thực hiện điểm danh trong ngày cho người dùng hiện tại.
    /// Luồng xử lý: lấy user id từ token, tạo command check-in, trả kết quả cập nhật streak.
    /// </summary>
    /// <returns>Kết quả check-in hoặc unauthorized khi không có user hợp lệ.</returns>
    [HttpPost]
    public async Task<IActionResult> DailyCheckIn()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn sớm khi không xác định được user để tránh ghi nhận check-in sai chủ thể.
            return this.UnauthorizedProblem();
        }

        var command = new DailyCheckInCommand { UserId = userId };
        var result = await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(result);
    }

    /// <summary>
    /// Lấy trạng thái streak hiện tại của người dùng.
    /// </summary>
    /// <returns>Thông tin streak hoặc unauthorized khi thiếu danh tính người dùng.</returns>
    [HttpGet("streak")]
    public async Task<IActionResult> GetStreakStatus()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Bảo đảm chỉ chủ thể đã xác thực mới đọc được dữ liệu streak của chính họ.
            return this.UnauthorizedProblem();
        }

        var query = new GetStreakStatusQuery { UserId = userId };
        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);
        return Ok(result);
    }

    /// <summary>
    /// Mua lượt đóng băng streak để bảo vệ chuỗi điểm danh.
    /// Luồng xử lý: xác thực user, gắn user id vào command, thực thi nghiệp vụ mua freeze.
    /// </summary>
    /// <param name="command">Command mua streak freeze.</param>
    /// <returns>Kết quả mua freeze hoặc unauthorized khi không có user id hợp lệ.</returns>
    [HttpPost("freeze")]
    public async Task<IActionResult> PurchaseFreeze([FromBody] PurchaseStreakFreezeCommand command)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn thao tác mua freeze khi không có danh tính để bảo toàn giao dịch ví.
            return this.UnauthorizedProblem();
        }

        // Bắt buộc ghi đè user id từ token để tránh client giả mạo chủ thể trong payload.
        command.UserId = userId; 

        var result = await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(result);
    }
}
