
namespace TarotNow.Domain.Enums;

// Tập hằng vai trò tài khoản trong hệ thống.
public static class UserRole
{
    // Vai trò người dùng thông thường.
    public const string User = "user";

    // Vai trò reader tư vấn Tarot.
    public const string TarotReader = "tarot_reader";

    // Vai trò quản trị viên.
    public const string Admin = "admin";

    // Vai trò hệ thống nội bộ.
    public const string System = "system";
}
