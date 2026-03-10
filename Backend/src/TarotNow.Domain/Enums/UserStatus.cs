namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái của tài khoản người dùng theo BR Phase 1.1.
/// - Pending: Đã đăng ký nhưng chưa xác minh mã OTP email.
/// - Active: Đã xác minh OTP thành công.
/// - Locked: Khóa do nhập sai mật khẩu quá nhiều lần (tương lai).
/// - Banned: Bị khóa thủ công do vi phạm TOS.
/// </summary>
public static class UserStatus
{
    public const string Pending = "pending";
    public const string Active = "active";
    public const string Locked = "locked";
    public const string Banned = "banned";
}
