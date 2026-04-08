using MediatR;
using System.Text.Json.Serialization;

namespace TarotNow.Application.Features.Admin.Queries.ListUsers;

// Query phân trang danh sách người dùng cho admin.
public class ListUsersQuery : IRequest<ListUsersResponse>
{
    // Trang hiện tại (1-based).
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;

    // Từ khóa tìm kiếm theo email/tên hiển thị (tùy triển khai repository).
    public string? SearchTerm { get; set; }
}

// Kết quả trả về cho truy vấn danh sách user.
public class ListUsersResponse
{
    // Danh sách user của trang hiện tại.
    public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();

    // Tổng số user theo bộ lọc.
    public int TotalCount { get; set; }
}

// DTO hiển thị user trong danh sách quản trị.
public class UserDto
{
    // Định danh user.
    public Guid Id { get; set; }

    // Email user.
    public string Email { get; set; } = string.Empty;

    // Tên hiển thị user.
    public string DisplayName { get; set; } = string.Empty;

    // Trạng thái tài khoản.
    public string Status { get; set; } = string.Empty;

    // Vai trò tài khoản.
    public string Role { get; set; } = string.Empty;

    // Cấp độ hiện tại của user.
    [JsonPropertyName("level")]
    public int Level { get; set; }

    // Điểm kinh nghiệm hiện tại.
    [JsonPropertyName("exp")]
    public long Exp { get; set; }

    // Số dư vàng.
    [JsonPropertyName("goldBalance")]
    public long GoldBalance { get; set; }

    // Số dư kim cương.
    [JsonPropertyName("diamondBalance")]
    public long DiamondBalance { get; set; }

    // Thời điểm tạo tài khoản.
    public DateTime CreatedAt { get; set; }
}
