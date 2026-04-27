using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Features.Profile.Commands.ConfirmAvatarUpload;
using TarotNow.Application.Features.Profile.Commands.PresignAvatarUpload;
using TarotNow.Application.Features.Profile.Commands.UpdateProfile;
using TarotNow.Application.Features.Profile.Queries.GetProfile;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Profile)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
// API hồ sơ người dùng.
// Luồng chính: lấy hồ sơ, cập nhật thông tin và quản lý avatar qua presign + confirm.
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller hồ sơ người dùng.
    /// </summary>
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy thông tin hồ sơ của người dùng hiện tại.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetProfileQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh mục ngân hàng hỗ trợ cấu hình payout.
    /// </summary>
    [HttpGet("payout-banks")]
    [Authorize]
    public IActionResult GetPayoutBanks()
    {
        var items = VietnamBankCatalog.GetAll()
            .Select(item => new
            {
                bankBin = item.Bin,
                bankName = item.Name
            });
        return Ok(items);
    }

    /// <summary>
    /// Cập nhật thông tin hồ sơ người dùng.
    /// </summary>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var success = await _mediator.SendWithRequestCancellation(HttpContext, new UpdateProfileCommand
        {
            UserId = userId,
            DisplayName = request.DisplayName,
            DateOfBirth = request.DateOfBirth,
            PayoutBankName = request.PayoutBankName,
            PayoutBankBin = request.PayoutBankBin,
            PayoutBankAccountNumber = request.PayoutBankAccountNumber,
            PayoutBankAccountHolder = request.PayoutBankAccountHolder
        });

        return success
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update profile",
                detail: "Không thể cập nhật hồ sơ người dùng.");
    }

    /// <summary>
    /// Sinh presigned URL upload avatar trực tiếp lên R2.
    /// </summary>
    [HttpPost("avatar/presign")]
    [Authorize]
    public async Task<IActionResult> PresignAvatar([FromBody] AvatarPresignRequest body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.SendWithRequestCancellation(HttpContext, new PresignAvatarUploadCommand
        {
            UserId = userId,
            ContentType = body.ContentType,
            SizeBytes = body.SizeBytes,
        });

        return Ok(new PresignedUploadResponse(
            result.UploadUrl,
            result.ObjectKey,
            result.PublicUrl,
            result.UploadToken,
            result.ExpiresAtUtc));
    }

    /// <summary>
    /// Xác nhận avatar upload thành công và cập nhật DB.
    /// </summary>
    [HttpPost("avatar/confirm")]
    [Authorize]
    public async Task<IActionResult> ConfirmAvatar([FromBody] AvatarConfirmRequest body)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.SendWithRequestCancellation(HttpContext, new ConfirmAvatarUploadCommand
        {
            UserId = userId,
            ObjectKey = body.ObjectKey,
            PublicUrl = body.PublicUrl,
            UploadToken = body.UploadToken,
        });

        return Ok(new
        {
            success = true,
            avatarUrl = result.AvatarUrl,
            objectKey = result.ObjectKey,
        });
    }
}
