using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;

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
        // Tỉ lệ chuyển đổi: 10,000 VND = 1 Diamond (Có thể tách ra config sau)
        long baseDiamondAmount = request.AmountVnd / 10000;
        
        // P1-PROMO-BE-1.2: Auto-apply promotion on deposit
        var activePromotions = await _promotionRepository.GetActivePromotionsAsync(cancellationToken);
        
        long bonusDiamond = 0;
        foreach (var promo in activePromotions)
        {
            if (request.AmountVnd >= promo.MinAmountVnd)
            {
                // activePromotions đã được sort descending theo MinAmountVnd
                // nên promotion đầu tiên thoả mãn sẽ là promotion tốt nhất
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
