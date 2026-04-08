using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TarotNow.Api.Constants;

/// <summary>
/// Tập trung tên policy và bộ rule ủy quyền dùng chung cho toàn bộ API.
/// Lý do: gom một nguồn sự thật giúp tránh lệch quyền giữa các controller.
/// </summary>
public static class ApiAuthorizationPolicies
{
    // Tên policy yêu cầu người dùng đã xác thực và có định danh hợp lệ.
    public const string AuthenticatedUser = "authenticated_user";

    // Tên policy chỉ cho phép tài khoản quản trị.
    public const string AdminOnly = "AdminOnly";

    /// <summary>
    /// Cấu hình policy cho nhóm người dùng đã đăng nhập.
    /// Luồng xử lý: 1) bắt buộc xác thực, 2) bắt buộc có claim định danh người dùng.
    /// </summary>
    public static readonly Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser = policy =>
    {
        // Bước xác thực nền để loại bỏ request ẩn danh ngay tại authorization pipeline.
        policy.RequireAuthenticatedUser();

        // Bổ sung claim định danh nhằm đảm bảo downstream luôn đọc được UserId an toàn.
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    };

    /// <summary>
    /// Cấu hình policy cho nhóm quản trị viên.
    /// Luồng xử lý: dùng nền tảng xác thực cơ bản rồi siết thêm role admin.
    /// </summary>
    public static readonly Action<AuthorizationPolicyBuilder> RequireAdminOnly = policy =>
    {
        // Giữ cùng baseline xác thực như policy người dùng để đồng nhất hành vi bảo mật.
        policy.RequireAuthenticatedUser();

        // Bắt buộc claim định danh để mọi thao tác quản trị đều truy vết được chủ thể.
        policy.RequireClaim(ClaimTypes.NameIdentifier);

        // Rule nghiệp vụ cuối: chỉ role admin mới được truy cập endpoint quản trị.
        policy.RequireRole("admin");
    };
}
