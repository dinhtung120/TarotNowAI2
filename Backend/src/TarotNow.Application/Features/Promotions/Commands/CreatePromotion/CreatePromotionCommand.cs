using MediatR;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommand : IRequest<bool>
{
    public long MinAmountVnd { get; set; }
    public long BonusDiamond { get; set; }
    public bool IsActive { get; set; }
}
