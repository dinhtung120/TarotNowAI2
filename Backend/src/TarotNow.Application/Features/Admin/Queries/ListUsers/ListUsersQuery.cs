/*
 * ===================================================================
 * FILE: ListUsersQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.ListUsers
 * ===================================================================
 * MỤC ĐÍCH:
 *   Query + Response DTO + UserDto cho Admin XEM DANH SÁCH USER.
 *   Trang "User Management" (quản lý người dùng) hiển thị:
 *   - Thông tin cơ bản (email, tên, role)
 *   - Số dư ví (gold, diamond)
 *   - Level + EXP (gamification)
 *   - Trạng thái (active, locked)
 *
 * TÌM KIẾM:
 *   SearchTerm: tìm theo email hoặc display name (partial match).
 *   Ví dụ: "nguyenvan" → tìm user có tên/email chứa "nguyenvan".
 *   Nullable → không truyền = lấy tất cả user.
 * ===================================================================
 */

using MediatR;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Admin.Queries.ListUsers;

/// <summary>Query lấy danh sách user (phân trang + tìm kiếm).</summary>
public class ListUsersQuery : IRequest<ListUsersResponse>
{
    /// <summary>Trang hiện tại (1-indexed). Mặc định trang 1.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Số item mỗi trang. Mặc định 20.</summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Từ khóa tìm kiếm (tìm trong email, display name).
    /// Nullable → không truyền = không filter.
    /// </summary>
    public string? SearchTerm { get; set; }
}

/// <summary>Response chứa danh sách user + tổng số.</summary>
public class ListUsersResponse
{
    /// <summary>Danh sách user (đã phân trang).</summary>
    public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();

    /// <summary>Tổng số user (tất cả trang).</summary>
    public int TotalCount { get; set; }
}

/// <summary>
/// DTO đại diện cho 1 user trong danh sách admin.
/// Chứa thông tin HIỂN THỊ, không chứa dữ liệu nhạy cảm (password hash).
/// </summary>
public class UserDto
{
    /// <summary>UUID user.</summary>
    public System.Guid Id { get; set; }

    /// <summary>Email (unique identifier).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Tên hiển thị.</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái: "active" (hoạt động) hoặc "locked" (bị khóa).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Quyền: "user" (người dùng) | "tarot_reader" (reader) | "admin".
    /// </summary>
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// Level gamification (cấp độ game).
    /// [JsonPropertyName("level")]: đảm bảo JSON trả "level" thay vì "Level".
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("level")]
    public int Level { get; set; }
    
    /// <summary>
    /// Experience points (điểm kinh nghiệm).
    /// EXP tích lũy → lên level → mở tính năng mới.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("exp")]
    public long Exp { get; set; }
    
    /// <summary>Số dư Gold (tiền miễn phí, nhận qua daily check-in).</summary>
    [System.Text.Json.Serialization.JsonPropertyName("goldBalance")]
    public long GoldBalance { get; set; }
    
    /// <summary>Số dư Diamond (tiền nạp, dùng cho dịch vụ trả phí).</summary>
    [System.Text.Json.Serialization.JsonPropertyName("diamondBalance")]
    public long DiamondBalance { get; set; }
    
    /// <summary>Thời điểm tạo tài khoản.</summary>
    public System.DateTime CreatedAt { get; set; }
}
