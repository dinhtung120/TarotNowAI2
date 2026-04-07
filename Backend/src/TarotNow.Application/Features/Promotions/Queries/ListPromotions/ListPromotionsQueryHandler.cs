

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
        
        
        var promotions = request.OnlyActive
            ? await _promotionRepository.GetActivePromotionsAsync(cancellationToken)
            : await _promotionRepository.GetAllAsync(cancellationToken);

        
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
