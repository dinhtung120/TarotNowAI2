

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Threading.Tasks;

using TarotNow.Api.Contracts; 
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Profile.Commands.UpdateProfile; 
using TarotNow.Application.Features.Profile.Commands.UploadAvatar;  
using TarotNow.Application.Features.Profile.Queries.GetProfile;     
using Microsoft.AspNetCore.Http;

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Profile)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
// API hồ sơ người dùng.
// Luồng chính: lấy hồ sơ, cập nhật thông tin và upload avatar.
public class ProfileController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Khởi tạo controller hồ sơ người dùng.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query/command profile.</param>
    public ProfileController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Lấy thông tin hồ sơ của người dùng hiện tại.
    /// </summary>
    /// <returns>Dữ liệu hồ sơ người dùng hoặc unauthorized khi thiếu user id.</returns>
    [HttpGet]
    [Authorize] 
    public async Task<IActionResult> GetProfile()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn hồ sơ khi không xác định được danh tính người dùng.
            return this.UnauthorizedProblem();
        }

        // Dựng query profile theo user id hiện tại để tránh lộ dữ liệu chéo tài khoản.
        var query = new GetProfileQuery { UserId = userId };

        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin hồ sơ người dùng.
    /// Luồng xử lý: xác thực user, map DTO sang command cập nhật, rẽ nhánh kết quả thành công/thất bại.
    /// </summary>
    /// <param name="request">Payload dữ liệu hồ sơ cần cập nhật.</param>
    /// <returns>Kết quả cập nhật hồ sơ.</returns>
    [HttpPatch]
    [Authorize] 
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn cập nhật hồ sơ khi không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Mapping rõ ràng để tầng ứng dụng xử lý validation/profile policy tập trung.
        var command = new UpdateProfileCommand
        {
            UserId = userId,                    
            DisplayName = request.DisplayName,  
            AvatarUrl = request.AvatarUrl,      
            DateOfBirth = request.DateOfBirth    
        };

        var success = await Mediator.Send(command);

        // Tách response lỗi để client biết cập nhật thất bại do rule nghiệp vụ.
        return success
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update profile",
                detail: "Không thể cập nhật hồ sơ người dùng.");
    }

    /// <summary>
    /// Upload ảnh đại diện mới.
    /// Luồng xử lý: xác thực user, kiểm tra file hợp lệ, gửi command xử lý ảnh và trả URL tương đối.
    /// </summary>
    /// <param name="file">File ảnh đại diện upload lên.</param>
    /// <returns>URL avatar sau khi xử lý thành công.</returns>
    [HttpPost("avatar")]
    [Authorize] 
    [RequestSizeLimit(10 * 1024 * 1024)] 
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn upload avatar khi chưa xác thực danh tính user.
            return this.UnauthorizedProblem();
        }

        if (file == null || file.Length == 0)
        {
            // Edge case file rỗng: trả lỗi rõ ràng để client chọn lại ảnh hợp lệ.
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid avatar file",
                detail: "File ảnh không được để trống.");
        }

        // Dùng stream để xử lý file trực tiếp, tránh đọc toàn bộ file vào bộ nhớ.
        using var stream = file.OpenReadStream();
        var command = new UploadAvatarCommand
        {
            UserId = userId,
            ImageStream = stream,
            FileName = file.FileName,
            ContentType = file.ContentType
        };

        var uploadResult = await Mediator.Send(command);

        return Ok(new
        {
            success = true,
            avatarUrl = uploadResult.AvatarUrl,
            publicId = uploadResult.PublicId,
        });
    }
}
