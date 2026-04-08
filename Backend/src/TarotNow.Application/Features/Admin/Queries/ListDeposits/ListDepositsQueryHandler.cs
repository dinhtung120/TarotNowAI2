using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

// Handler truy vấn danh sách lệnh nạp tiền cho admin.
public class ListDepositsQueryHandler : IRequestHandler<ListDepositsQuery, ListDepositsResponse>
{
    private const string UnknownUsername = "Unknown";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler list deposits.
    /// Luồng xử lý: nhận repository deposit order và user để lấy dữ liệu + ánh xạ username.
    /// </summary>
    public ListDepositsQueryHandler(IDepositOrderRepository depositOrderRepository, IUserRepository userRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý query lấy danh sách lệnh nạp theo phân trang và trạng thái.
    /// Luồng xử lý: lấy orders paginated, tải map username theo user id, map sang response DTO.
    /// </summary>
    public async Task<ListDepositsResponse> Handle(ListDepositsQuery request, CancellationToken cancellationToken)
    {
        var (orders, totalCount) = await _depositOrderRepository.GetPaginatedAsync(
            request.Page, request.PageSize, request.Status, cancellationToken);

        var userIds = orders.Select(order => order.UserId).Distinct().ToList();
        var userMap = await _userRepository.GetUsernameMapAsync(userIds, cancellationToken);

        return new ListDepositsResponse
        {
            Deposits = MapDeposits(orders, userMap),
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Ánh xạ entity deposit order sang DTO trả về cho API admin.
    /// Luồng xử lý: map từng order và fallback username "Unknown" khi không tra được trong user map.
    /// </summary>
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
