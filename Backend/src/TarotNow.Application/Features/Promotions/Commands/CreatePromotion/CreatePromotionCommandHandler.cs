using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    public CreatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<bool> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = new DepositPromotion(request.MinAmountVnd, request.BonusDiamond, request.IsActive);
        
        await _promotionRepository.AddAsync(promotion, cancellationToken);
        
        return true;
    }
}
