/*
 * ===================================================================
 * FILE: GetLedgerMismatchQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler thực thi query đối soát sổ cái.
 *   Gọi repository → lấy dữ liệu thô → chuyển sang DTO.
 *
 * KIẾN TRÚC:
 *   Handler KHÔNG biết SQL hay stored procedure.
 *   Chỉ gọi IAdminRepository.GetLedgerMismatchesAsync().
 *   Repository (Infrastructure layer) thực hiện SQL thực tế:
 *     SELECT u.id, u.gold_balance, SUM(l.amount) ...
 *     HAVING u.gold_balance <> SUM(l.amount)
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

/// <summary>
/// Handler xử lý query đối soát sổ cái.
/// IRequestHandler<Query, Response>: interface MediatR.
/// </summary>
public class GetLedgerMismatchQueryHandler : IRequestHandler<GetLedgerMismatchQuery, List<MismatchRecordDto>>
{
    private readonly IAdminRepository _adminRepository;

    public GetLedgerMismatchQueryHandler(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    /// <summary>
    /// Thực thi query: lấy danh sách user có số dư lệch sổ cái.
    ///
    /// Luồng:
    ///   1. Gọi repository lấy dữ liệu thô (entity)
    ///   2. Map entity → DTO (sử dụng LINQ Select)
    ///   3. Trả danh sách DTO cho controller
    ///
    /// LINQ Select: chuyển đổi từ kiểu A sang kiểu B.
    ///   entities.Select(e => new DTO { ... }): mỗi entity → 1 DTO.
    ///   .ToList(): thực thi truy vấn và tạo danh sách.
    /// </summary>
    public async Task<List<MismatchRecordDto>> Handle(GetLedgerMismatchQuery request, CancellationToken cancellationToken)
    {
        // Repository thực hiện SQL query phức tạp (JOIN + GROUP BY + HAVING)
        var entities = await _adminRepository.GetLedgerMismatchesAsync(cancellationToken);
        
        // Map entity → DTO (Application layer chỉ làm việc với DTO)
        return entities.Select(e => new MismatchRecordDto
        {
            UserId = e.UserId,
            UserGoldBalance = e.UserGoldBalance,
            LedgerGoldBalance = e.LedgerGoldBalance,
            UserDiamondBalance = e.UserDiamondBalance,
            LedgerDiamondBalance = e.LedgerDiamondBalance
        }).ToList();
    }
}
