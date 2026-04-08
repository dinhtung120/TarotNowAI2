namespace TarotNow.Api.Constants;

/// <summary>
/// Tập trung định nghĩa role để policy và authorize attribute dùng thống nhất.
/// Lý do: tránh hardcode chuỗi role rải rác gây sai khác phân quyền.
/// </summary>
public static class ApiRoleConstants
{
    // Role người dùng ứng dụng thông thường.
    public const string User = "user";

    // Role reader thực hiện phiên tư vấn.
    public const string TarotReader = "tarot_reader";

    // Role quản trị viên có quyền vận hành hệ thống.
    public const string Admin = "admin";

    // Role nội bộ cho tác vụ hệ thống hoặc automation.
    public const string System = "system";
}
