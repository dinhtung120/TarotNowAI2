

using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

public class ListDepositsQueryHandler : IRequestHandler<ListDepositsQuery, ListDepositsResponse>
{
    private const string UnknownUsername = "Unknown";

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
            Deposits = MapDeposits(orders, userMap),
            TotalCount = totalCount
        };
    }

    private static List<DepositDto> MapDeposits(
        IEnumerable<Domain.Entities.DepositOrder> orders,
        IReadOnlyDictionary<Guid, string> userMap)
    {
        return orders.Select(order => new DepositDto
        {
            Id = order.Id,
            UserId = order.UserId,
            Username = userMap.TryGetValue(order.UserId, out var username)
                ? username
                : UnknownUsername,
            AmountVnd = order.AmountVnd,
            DiamondAmount = order.DiamondAmount,
            Status = order.Status,
            TransactionId = order.TransactionId,
            CreatedAt = order.CreatedAt
        }).ToList();
    }
}
