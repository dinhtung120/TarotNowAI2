using MediatR;
using System;

namespace TarotNow.Application.Features.Profile.Queries.GetProfile;

// Query lấy hồ sơ chi tiết của người dùng hiện tại.
public class GetProfileQuery : IRequest<ProfileResponse>
{
    // Định danh user cần tải profile.
    public Guid UserId { get; set; }
}

// DTO hồ sơ người dùng trả về cho client.
public class ProfileResponse
{
    // Định danh user.
    public Guid Id { get; set; }

    // Email tài khoản.
    public string Email { get; set; } = string.Empty;

    // Username duy nhất của tài khoản.
    public string Username { get; set; } = string.Empty;

    // Tên hiển thị trên hệ thống.
    public string DisplayName { get; set; } = string.Empty;

    // URL avatar hiện tại (nếu có).
    public string? AvatarUrl { get; set; }

    // Ngày sinh của user.
    public DateTime DateOfBirth { get; set; }

    // Cung hoàng đạo suy ra từ ngày sinh.
    public string Zodiac { get; set; } = string.Empty;

    // Chỉ số thần số học suy ra từ ngày sinh.
    public int Numerology { get; set; }

    // Cấp độ hiện tại của user.
    public int Level { get; set; }

    // Điểm kinh nghiệm hiện tại.
    public long Exp { get; set; }

    // Cờ cho biết user đã đồng ý đầy đủ bộ tài liệu pháp lý bắt buộc hay chưa.
    public bool HasConsented { get; set; }
}
