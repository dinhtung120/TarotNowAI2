using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListUsers;

// Handler truy vấn danh sách người dùng phân trang.
public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, ListUsersResponse>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler list users.
    /// Luồng xử lý: nhận user repository để lấy danh sách user theo phân trang và từ khóa.
    /// </summary>
    public ListUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý query lấy danh sách user cho admin.
    /// Luồng xử lý: truy vấn paginated users từ repository, map sang DTO, trả kèm total count.
    /// </summary>
    public async Task<ListUsersResponse> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetPaginatedUsersAsync(
            request.Page, request.PageSize, request.SearchTerm, cancellationToken);

        return new ListUsersResponse
        {
            // Map explicit để khóa contract trả về và tránh lộ fields không cần thiết.
            Users = users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Status = user.Status,
                Role = user.Role,
                Level = user.Level,
                Exp = user.Exp,
                GoldBalance = user.GoldBalance,
                DiamondBalance = user.DiamondBalance,
                CreatedAt = user.CreatedAt
            }),
            TotalCount = totalCount
        };
    }
}
