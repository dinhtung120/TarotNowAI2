/*
 * ===================================================================
 * FILE: ListUsersQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.ListUsers
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý query lấy danh sách user cho admin.
 *   Lấy user từ PostgreSQL → map entity → DTO → trả về.
 *
 * ENTITY → DTO MAPPING:
 *   User entity (Domain layer) chứa TẤT CẢ thông tin: password hash, refresh token...
 *   UserDto (Application layer) chỉ chứa thông tin HIỂN THỊ.
 *   Mapping đảm bảo KHÔNG rò rỉ dữ liệu nhạy cảm ra ngoài.
 * ===================================================================
 */

using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListUsers;

/// <summary>Handler lấy danh sách user phân trang.</summary>
public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, ListUsersResponse>
{
    private readonly IUserRepository _userRepository;

    public ListUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ListUsersResponse> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        /*
         * GetPaginatedUsersAsync: lấy user phân trang từ PostgreSQL.
         * Repository thực hiện:
         *   1. Filter bằng SearchTerm (LIKE '%term%' trên email, display_name)
         *   2. Đếm tổng (COUNT)
         *   3. Skip + Take cho phân trang
         *   4. Sort theo CreatedAt descending (mặc định)
         * 
         * Tuple deconstruction → 2 biến: users (danh sách), totalCount (tổng).
         */
        var (users, totalCount) = await _userRepository.GetPaginatedUsersAsync(
            request.Page, request.PageSize, request.SearchTerm, cancellationToken);
        
        // Map User entity → UserDto (chỉ lấy thông tin hiển thị)
        return new ListUsersResponse
        {
            /*
             * LINQ Select: chuyển từ User (entity) sang UserDto.
             * Mỗi user entity → 1 UserDto (chỉ copy các field cần hiển thị).
             * 
             * KHÔNG copy: PasswordHash, RefreshToken, MfaSecret...
             * (những field nhạy cảm không nên trả về cho client)
             * 
             * Lưu ý: Select trả IEnumerable (lazy), không cần ToList()
             * vì ListUsersResponse.Users kiểu IEnumerable<UserDto>.
             */
            Users = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                DisplayName = u.DisplayName,
                Status = u.Status,
                Role = u.Role,
                Level = u.Level,
                Exp = u.Exp,
                GoldBalance = u.GoldBalance,
                DiamondBalance = u.DiamondBalance,
                CreatedAt = u.CreatedAt
            }),
            TotalCount = totalCount
        };
    }
}
