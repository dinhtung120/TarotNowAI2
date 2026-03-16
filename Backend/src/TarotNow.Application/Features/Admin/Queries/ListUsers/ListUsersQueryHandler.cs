using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListUsers;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, ListUsersResponse>
{
    private readonly IUserRepository _userRepository;

    public ListUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ListUsersResponse> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        // IUserRepository currently only has GetById, GetByEmail, Add, Update.
        // I will need to extend it to support get all paginated if it's not present.
        // Assuming GetPaginatedAsync exists. If not, I'll need to add it or use DbSet directly (which breaks CA).
        // I will implement a placeholder that relies on GetPaginatedUsersAsync.
        var (users, totalCount) = await _userRepository.GetPaginatedUsersAsync(request.Page, request.PageSize, request.SearchTerm, cancellationToken);
        
        return new ListUsersResponse
        {
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
