/*
 * ===================================================================
 * FILE: ProfileController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý HỒ SƠ CÁ NHÂN (Profile) của người dùng.
 *   Cho phép:
 *   1. Xem thông tin hồ sơ (tên, email, ảnh đại diện, ngày sinh)
 *   2. Cập nhật thông tin hồ sơ
 *
 * ĐƠN GIẢN NHẤT TRONG CÁC CONTROLLER:
 *   Chỉ có 2 endpoint, logic đơn giản, không liên quan tài chính.
 *   Minh họa rõ nhất pattern "Thin Controller" + MediatR.
 * ===================================================================
 */

using MediatR;                 // MediatR trung gian
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền
using Microsoft.AspNetCore.Mvc; // API controller
using System;
using System.Threading.Tasks;

using TarotNow.Api.Contracts; // Import UpdateProfileRequest DTO
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Profile.Commands.UpdateProfile; // Command cập nhật
using TarotNow.Application.Features.Profile.Queries.GetProfile;     // Query lấy profile

namespace TarotNow.Api.Controllers;

/*
 * [Route("api/v1/profile")]: URL gốc = /api/v1/profile
 * KHÔNG dùng [controller] vì tên tường minh hơn (profile thay vì Profile).
 */
[Route("api/v1/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator Mediator;

    public ProfileController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/profile
    /// MỤC ĐÍCH: Lấy thông tin hồ sơ cá nhân của user đang đăng nhập.
    ///
    /// TRẢ VỀ:
    ///   - Email, username, displayName (tên hiển thị)
    ///   - AvatarUrl (ảnh đại diện)
    ///   - DateOfBirth (ngày sinh)
    ///   - Role (vai trò: user/reader/admin)
    ///   - Balances (số dư ví)
    ///   - Trạng thái tài khoản (active/locked)
    ///
    /// KHÔNG CÓ THAM SỐ:
    ///   Server tự biết xem profile CỦA AI nhờ JWT token.
    ///   User A không thể xem profile User B (trừ public profile khác).
    /// </summary>
    [HttpGet]
    [Authorize] // Phải đăng nhập
    public async Task<IActionResult> GetProfile()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        // Tạo query "Lấy profile của user với ID này"
        var query = new GetProfileQuery { UserId = userId };

        // Gửi qua MediatR → handler truy vấn database → trả profile
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: PATCH /api/v1/profile
    /// MỤC ĐÍCH: Cập nhật thông tin hồ sơ cá nhân.
    ///
    /// [HttpPatch]: Dùng PATCH thay vì PUT vì chỉ cập nhật MỘT PHẦN.
    ///   - PATCH: "Sửa một vài trường" (displayName, avatar, dateOfBirth)
    ///   - PUT: "Thay thế TOÀN BỘ resource" (ít dùng cho profile)
    ///
    /// DỮ LIỆU CÓ THỂ CẬP NHẬT:
    ///   - DisplayName: tên hiển thị
    ///   - AvatarUrl: đường dẫn ảnh đại diện
    ///   - DateOfBirth: ngày sinh
    ///
    /// KHÔNG CHO CẬP NHẬT:
    ///   - Email (cần luồng verify riêng)
    ///   - Password (dùng reset-password)
    ///   - Role (chỉ admin mới đổi được)
    /// </summary>
    [HttpPatch]
    [Authorize] // Phải đăng nhập
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        // Tạo command cập nhật profile
        // userId từ JWT (bảo mật) + dữ liệu từ request body (user gửi)
        var command = new UpdateProfileCommand
        {
            UserId = userId,                    // AI dùng userId từ JWT
            DisplayName = request.DisplayName,  // Tên mới (từ body)
            AvatarUrl = request.AvatarUrl,      // Ảnh mới (từ body, nullable)
            DateOfBirth = request.DateOfBirth    // Ngày sinh mới (từ body)
        };

        // Gửi command → handler validate + cập nhật database
        var success = await Mediator.Send(command);

        // Trả kết quả: thành công hoặc thất bại
        return success ? Ok(new { success = true }) : BadRequest();
    }
}
