using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    /// Gửi đơn đăng ký trở thành Reader cho người dùng hiện tại.
    /// </summary>
    [HttpPost("apply")]
    [Authorize]
    public async Task<IActionResult> Apply([FromBody] SubmitReaderRequestBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new SubmitReaderRequestCommand
        {
            UserId = userId,
            IntroText = body.IntroText,
            ProofDocuments = body.ProofDocuments ?? []
        });

        return result
            ? Ok(new { success = true, message = "Đơn đã được gửi thành công. Vui lòng chờ admin duyệt." })
            : BadRequest(new { message = "Không thể gửi đơn." });
    }

    /// <summary>
    /// Lấy trạng thái đơn đăng ký Reader của người dùng hiện tại.
    /// </summary>
    [HttpGet("my-request")]
    [Authorize]
    public async Task<IActionResult> GetMyRequest()
    {
        if (!User.TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new GetMyReaderRequestQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin hồ sơ Reader của tài khoản hiện tại.
    /// </summary>
    [HttpPatch("profile")]
    [Authorize(Roles = ApiRoleConstants.TarotReader)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateReaderProfileBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new UpdateReaderProfileCommand
        {
            UserId = userId,
            BioVi = body.BioVi,
            BioEn = body.BioEn,
            BioZh = body.BioZh,
            DiamondPerQuestion = body.DiamondPerQuestion,
            Specialties = body.Specialties
        });

        return result ? Ok(new { success = true }) : BadRequest();
    }

    /// <summary>
    /// Cập nhật trạng thái hoạt động thủ công của Reader (busy hoặc offline).
    /// </summary>
    [HttpPatch("status")]
    [Authorize(Roles = ApiRoleConstants.TarotReader)]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateReaderStatusBody body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new UpdateReaderStatusCommand
        {
            UserId = userId,
            Status = body.Status
        });

        return result ? Ok(new { success = true }) : BadRequest();
    }
}
