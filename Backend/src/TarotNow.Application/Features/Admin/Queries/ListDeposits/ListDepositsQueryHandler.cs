using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

public class ListDepositsQueryHandler : IRequestHandler<ListDepositsQuery, ListDepositsResponse>
{
    private readonly IDepositOrderRepository _depositOrderRepository;

    public ListDepositsQueryHandler(IDepositOrderRepository depositOrderRepository)
    {
        _depositOrderRepository = depositOrderRepository;
    }

    public async Task<ListDepositsResponse> Handle(ListDepositsQuery request, CancellationToken cancellationToken)
    {
        var (orders, totalCount) = await _depositOrderRepository.GetPaginatedAsync(request.Page, request.PageSize, request.Status, cancellationToken);
        
        return new ListDepositsResponse
        {
            Deposits = orders.Select(o => new DepositDto
            {
                Id = o.Id,
                UserId = o.UserId,
                AmountVnd = o.AmountVnd,
                DiamondAmount = o.DiamondAmount,
                Status = o.Status,
                TransactionId = o.TransactionId,
                CreatedAt = o.CreatedAt
            }),
            TotalCount = totalCount
        };
    }
}
