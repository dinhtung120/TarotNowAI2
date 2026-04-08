using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Queries.ListPromotions;

// Handler truy vấn danh sách promotion.
public class ListPromotionsQueryHandler : IRequestHandler<ListPromotionsQuery, IEnumerable<PromotionResponse>>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler lấy danh sách promotion.
    /// Luồng xử lý: nhận promotion repository để tải tập dữ liệu theo nhu cầu lọc active/all.
    /// </summary>
    public ListPromotionsQueryHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý query liệt kê promotion.
    /// Luồng xử lý: chọn nguồn dữ liệu theo cờ OnlyActive rồi map entity sang DTO response.
    /// </summary>
    public async Task<IEnumerable<PromotionResponse>> Handle(ListPromotionsQuery request, CancellationToken cancellationToken)
    {
        var promotions = request.OnlyActive
            ? await _promotionRepository.GetActivePromotionsAsync(cancellationToken)
            : await _promotionRepository.GetAllAsync(cancellationToken);
        // Nhánh lọc dữ liệu: dùng query tối ưu theo trạng thái active thay vì lọc tay sau khi lấy toàn bộ.

        return promotions.Select(promotion => new PromotionResponse
        {
            Id = promotion.Id,
            MinAmountVnd = promotion.MinAmountVnd,
            BonusDiamond = promotion.BonusDiamond,
            IsActive = promotion.IsActive,
            CreatedAt = promotion.CreatedAt
        });
    }
}
