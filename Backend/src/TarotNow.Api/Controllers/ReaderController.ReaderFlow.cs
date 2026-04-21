using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest;

namespace TarotNow.Api.Controllers;

public partial class ReaderController
{
    /// <summary>
    /// Gửi đơn đăng ký trở thành reader.
    /// Luồng xử lý: xác thực user, gửi command submit request, rẽ nhánh success/failure.
    /// </summary>
    /// <param name="body">Payload nội dung đơn đăng ký.</param>
    /// <returns>Kết quả gửi đơn đăng ký reader.</returns>
    [HttpPost("apply")]
    [Authorize]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> Apply([FromBody] SubmitReaderRequestBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn gửi đơn khi không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new SubmitReaderRequestCommand
        {
            UserId = userId,
            Bio = body.Bio,
            Specialties = body.Specialties,
            YearsOfExperience = body.YearsOfExperience,
            FacebookUrl = body.FacebookUrl,
            InstagramUrl = body.InstagramUrl,
            TikTokUrl = body.TikTokUrl,
            DiamondPerQuestion = body.DiamondPerQuestion,
            ProofDocuments = body.ProofDocuments ?? []
        });

        // Tách nhánh lỗi nghiệp vụ để client hiển thị đúng thông điệp cho người dùng.
        return result
            ? Ok(new { success = true, message = "Đơn đã được gửi thành công. Vui lòng chờ admin duyệt." })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot submit reader request",
                detail: "Không thể gửi đơn.");
    }

    /// <summary>
    /// Lấy trạng thái đơn đăng ký reader của chính người dùng hiện tại.
    /// </summary>
    /// <returns>Thông tin đơn đăng ký hiện tại hoặc unauthorized khi thiếu user id.</returns>
    [HttpGet("my-request")]
    [Authorize]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> GetMyRequest()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn đơn cá nhân khi không xác thực.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new GetMyReaderRequestQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật hồ sơ reader đã được duyệt.
    /// Luồng xử lý: xác thực reader, map payload sang command, rẽ nhánh kết quả.
    /// </summary>
    /// <param name="body">Payload cập nhật hồ sơ reader.</param>
    /// <returns>Kết quả cập nhật hồ sơ reader.</returns>
    [HttpPatch("profile")]
    [Authorize(Roles = ApiRoleConstants.TarotReader)]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateReaderProfileBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn cập nhật hồ sơ reader khi danh tính không hợp lệ.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new UpdateReaderProfileCommand
        {
            UserId = userId,
            BioVi = body.BioVi,
            BioEn = body.BioEn,
            BioZh = body.BioZh,
            DiamondPerQuestion = body.DiamondPerQuestion,
            Specialties = body.Specialties,
            YearsOfExperience = body.YearsOfExperience,
            FacebookUrl = body.FacebookUrl,
            InstagramUrl = body.InstagramUrl,
            TikTokUrl = body.TikTokUrl
        });

        // Trả lỗi business khi cập nhật thất bại để client hiển thị thông báo phù hợp.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update reader profile",
                detail: "Không thể cập nhật hồ sơ reader.");
    }

    /// <summary>
    /// Cập nhật trạng thái hoạt động của reader.
    /// Luồng xử lý: xác thực reader, gửi command cập nhật status, trả nhánh kết quả tương ứng.
    /// </summary>
    /// <param name="body">Payload trạng thái reader mới.</param>
    /// <returns>Kết quả cập nhật trạng thái reader.</returns>
    [HttpPatch("status")]
    [Authorize(Roles = ApiRoleConstants.TarotReader)]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateReaderStatusBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn cập nhật status khi không xác định được chủ thể reader.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new UpdateReaderStatusCommand
        {
            UserId = userId,
            Status = body.Status
        });

        // Tách nhánh lỗi để phản ánh rõ thất bại cập nhật trạng thái.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update reader status",
                detail: "Không thể cập nhật trạng thái reader.");
    }
}
