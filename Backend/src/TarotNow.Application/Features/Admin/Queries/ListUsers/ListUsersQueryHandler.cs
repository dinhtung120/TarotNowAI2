

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
        
        var (users, totalCount) = await _userRepository.GetPaginatedUsersAsync(
            request.Page, request.PageSize, request.SearchTerm, cancellationToken);
        
        
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
