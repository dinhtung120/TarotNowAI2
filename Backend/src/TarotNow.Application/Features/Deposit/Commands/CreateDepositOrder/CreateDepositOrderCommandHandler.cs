

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Constants;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

public class CreateDepositOrderCommandHandler : IRequestHandler<CreateDepositOrderCommand, CreateDepositOrderResponse>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IDepositPromotionRepository _promotionRepository;

    public CreateDepositOrderCommandHandler(IDepositOrderRepository depositOrderRepository, IDepositPromotionRepository promotionRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _promotionRepository = promotionRepository;
    }

    public async Task<CreateDepositOrderResponse> Handle(CreateDepositOrderCommand request, CancellationToken cancellationToken)
    {
        
        
        
        
        long baseDiamondAmount = request.AmountVnd / EconomyConstants.VndPerDiamond;
        
        
        
        
        
        
        var activePromotions = await _promotionRepository.GetActivePromotionsAsync(cancellationToken);
        
        long bonusDiamond = 0;
        foreach (var promo in activePromotions)
        {
            
            
            if (request.AmountVnd >= promo.MinAmountVnd)
            {
                bonusDiamond = promo.BonusDiamond;
                break; 
            }
        }

        long totalDiamondAmount = baseDiamondAmount + bonusDiamond;

        
        
        
        
        
        var order = new DepositOrder(request.UserId, request.AmountVnd, totalDiamondAmount);

        await _depositOrderRepository.AddAsync(order, cancellationToken);

        return new CreateDepositOrderResponse
        {
            OrderId = order.Id,
            AmountVnd = order.AmountVnd,
            DiamondAmount = order.DiamondAmount
        };
    }
}
