using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Queries.ListPromotions;

// Handler lấy danh sách promotion.
public class ListPromotionsQueryHandler : IRequestHandler<ListPromotionsQuery, IEnumerable<PromotionResponse>>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler list promotions.
    /// </summary>
    public ListPromotionsQueryHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý query lấy danh sách promotion.
    /// </summary>
    public async Task<IEnumerable<PromotionResponse>> Handle(ListPromotionsQuery request, CancellationToken cancellationToken)
    {
        var promotions = request.OnlyActive
            ? await _promotionRepository.GetActivePromotionsAsync(cancellationToken)
            : await _promotionRepository.GetAllAsync(cancellationToken);

        return promotions.Select(promotion => new PromotionResponse
        {
            Id = promotion.Id,
            MinAmountVnd = promotion.MinAmountVnd,
            BonusGold = promotion.BonusGold,
            IsActive = promotion.IsActive,
            CreatedAt = promotion.CreatedAt
        }).ToList();
    }
}
