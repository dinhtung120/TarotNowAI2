

using MediatR;
using System;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    
        public long MinAmountVnd { get; set; }
    
        public long BonusDiamond { get; set; }
    
        public bool IsActive { get; set; }
}
