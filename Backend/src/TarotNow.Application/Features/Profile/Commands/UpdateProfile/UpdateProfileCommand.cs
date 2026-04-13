using MediatR;
using System;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

// Command cập nhật thông tin hồ sơ người dùng.
public class UpdateProfileCommand : IRequest<bool>
{
    // Định danh user cần cập nhật hồ sơ.
    public Guid UserId { get; set; }

    // Tên hiển thị mới của user.
    public string DisplayName { get; set; } = string.Empty;

    // Ngày sinh dùng cho hồ sơ và các tính toán metadata (zodiac/numerology).
    public DateTime DateOfBirth { get; set; }
}
