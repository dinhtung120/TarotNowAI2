
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái tài khoản người dùng.
public static class UserStatus
{
    // Tài khoản mới tạo, chưa kích hoạt đầy đủ.
    public const string Pending = "pending";

    // Tài khoản đang hoạt động.
    public const string Active = "active";

    // Tài khoản bị khóa tạm thời.
    public const string Locked = "locked";

    // Tài khoản bị cấm vĩnh viễn.
    public const string Banned = "banned";
}
