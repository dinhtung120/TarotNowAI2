/*
 * ===================================================================
 * FILE: ListPromotionsQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Queries.ListPromotions
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thi hành việc Truy vấn danh sách Khuyến Mãi.
 *   Xử lý rẽ nhánh DB Query dựa vào cờ `OnlyActive` của DTO.
 * ===================================================================
 */

using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Queries.ListPromotions;

public class ListPromotionsQueryHandler : IRequestHandler<ListPromotionsQuery, IEnumerable<PromotionResponse>>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    public ListPromotionsQueryHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<IEnumerable<PromotionResponse>> Handle(ListPromotionsQuery request, CancellationToken cancellationToken)
    {
        // 1. Chĩa ngã ba đường (Rẽ nhánh Truy Vấn).
        // Tối ưu Tốc Độ Bằng Cách Chỉ Kéo Dữ Liệu Cần Thiết (Khách hàng thì đừng cho họ xem Khuyến mãi đã Tắt).
        var promotions = request.OnlyActive
            ? await _promotionRepository.GetActivePromotionsAsync(cancellationToken)
            : await _promotionRepository.GetAllAsync(cancellationToken);

        // 2. Map từ Model Gốc sang View Model.
        return promotions.Select(p => new PromotionResponse
        {
            Id = p.Id,
            MinAmountVnd = p.MinAmountVnd,
            BonusDiamond = p.BonusDiamond,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt
        });
    }
}
