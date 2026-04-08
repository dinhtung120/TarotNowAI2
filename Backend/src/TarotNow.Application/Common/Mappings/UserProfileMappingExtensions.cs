using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Common.Mappings;

// Nhóm extension mapping User domain entity sang DTO trả về cho luồng xác thực/hồ sơ.
public static class UserProfileMappingExtensions
{
    /// <summary>
    /// Chuyển đổi thực thể người dùng sang DTO hồ sơ dùng ở tầng application/API.
    /// Luồng xử lý: ánh xạ trực tiếp các trường cốt lõi và chuẩn hóa trạng thái enum thành chuỗi.
    /// </summary>
    public static UserProfileDto ToUserProfileDto(this User user)
    {
        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Level = user.Level,
            Exp = user.Exp,
            Role = user.Role,
            // Chuẩn hóa enum sang string để payload ổn định cho client và tránh phụ thuộc giá trị số enum.
            Status = user.Status.ToString()
        };
    }
}
