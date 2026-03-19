/*
 * ===================================================================
 * FILE: ListDepositsQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.ListDeposits
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler thực thi query lấy danh sách đơn nạp tiền.
 *   Kết hợp DỮ LIỆU TỪ 2 NGUỒN:
 *     1. deposit_orders (PostgreSQL) → thông tin đơn nạp
 *     2. users (PostgreSQL) → tên user (username)
 *
 * KỸ THUẬT BATCH LOADING:
 *   Thay vì n+1 query (mỗi đơn → 1 query lấy username):
 *   → Gộp tất cả userId → 1 query lấy hết username → map bằng Dictionary
 *   → Tổng chỉ 2 query thay vì n+1 query = HIỆU SUẤT TỐT HƠN NHIỀU
 * ===================================================================
 */

using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

/// <summary>Handler xử lý query lấy danh sách đơn nạp tiền.</summary>
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
        /*
         * Bước 1: Lấy đơn nạp tiền (phân trang + filter).
         * GetPaginatedAsync trả về Tuple:
         *   orders: danh sách đơn trang hiện tại
         *   totalCount: tổng số đơn (tất cả trang)
         * 
         * "var (orders, totalCount) = ...": tuple deconstruction.
         * Tách tuple thành 2 biến riêng biệt trong 1 dòng.
         */
        var (orders, totalCount) = await _depositOrderRepository.GetPaginatedAsync(
            request.Page, request.PageSize, request.Status, cancellationToken);
        
        /*
         * Bước 2: BATCH LOADING username — Kỹ thuật chống N+1 query.
         * 
         * N+1 Query Problem:
         *   20 đơn nạp → 1 query lấy đơn + 20 query lấy username = 21 queries!
         *   Với 100 đơn → 101 queries → RẤT CHẬM
         * 
         * Giải pháp: Batch loading
         *   1. Lấy tất cả userId duy nhất: .Distinct()
         *   2. 1 query lấy hết username: WHERE id IN (...)
         *   3. Map bằng Dictionary: O(1) lookup
         *   → Tổng: 2 queries (đơn nạp + username) → NHANH HƠN RẤT NHIỀU
         */
        var userIds = orders.Select(o => o.UserId).Distinct().ToList();
        var userMap = await _userRepository.GetUsernameMapAsync(userIds, cancellationToken);

        // Bước 3: Map entity → DTO, kết hợp username từ Dictionary
        return new ListDepositsResponse
        {
            Deposits = orders.Select(o => new DepositDto
            {
                Id = o.Id,
                UserId = o.UserId,
                /*
                 * TryGetValue: tìm username trong Dictionary.
                 *   Nếu tìm thấy → name = username, trả "name"
                 *   Nếu không thấy → trả "Unknown" (user bị xóa?)
                 * Pattern này an toàn hơn Dictionary[key] (throw nếu không có key).
                 */
                Username = userMap.TryGetValue(o.UserId, out var name) ? name : "Unknown",
                AmountVnd = o.AmountVnd,
                DiamondAmount = o.DiamondAmount,
                Status = o.Status,
                TransactionId = o.TransactionId,
                CreatedAt = o.CreatedAt
            }).ToList(), // ToList() materializes query → tạo List cụ thể trong memory
            TotalCount = totalCount
        };
    }
}
