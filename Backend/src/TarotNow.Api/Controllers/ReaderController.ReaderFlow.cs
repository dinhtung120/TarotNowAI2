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
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot submit reader request",
                detail: "Không thể gửi đơn.");
    }

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

        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update reader profile",
                detail: "Không thể cập nhật hồ sơ reader.");
    }

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

        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update reader status",
                detail: "Không thể cập nhật trạng thái reader.");
    }
}
