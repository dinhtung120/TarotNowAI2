using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandHandler : IRequestHandler<UpdatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    public UpdatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<bool> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Promotion {request.Id} not found");

        promotion.Update(request.MinAmountVnd, request.BonusDiamond, request.IsActive);
        
        await _promotionRepository.UpdateAsync(promotion, cancellationToken);
        
        return true;
    }
}
