

using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

public class ListDepositsQueryHandler : IRequestHandler<ListDepositsQuery, ListDepositsResponse>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IUserRepository _userRepository;

    public ListDepositsQueryHandler(IDepositOrderRepository depositOrderRepository, IUserRepository userRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _userRepository = userRepository;
    }

    public async Task<ListDepositsResponse> Handle(ListDepositsQuery request, CancellationToken cancellationToken)
    {
        
        var (orders, totalCount) = await _depositOrderRepository.GetPaginatedAsync(
            request.Page, request.PageSize, request.Status, cancellationToken);
        
        
        var userIds = orders.Select(o => o.UserId).Distinct().ToList();
        var userMap = await _userRepository.GetUsernameMapAsync(userIds, cancellationToken);

        
        return new ListDepositsResponse
        {
            Deposits = orders.Select(o => new DepositDto
            {
                Id = o.Id,
                UserId = o.UserId,
                
                Username = userMap.TryGetValue(o.UserId, out var name) ? name : "Unknown",
                AmountVnd = o.AmountVnd,
                DiamondAmount = o.DiamondAmount,
                Status = o.Status,
                TransactionId = o.TransactionId,
                CreatedAt = o.CreatedAt
            }).ToList(), 
            TotalCount = totalCount
        };
    }
}
