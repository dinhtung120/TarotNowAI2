

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Promotions.Queries.ListPromotions;

public class ListPromotionsQuery : IRequest<IEnumerable<PromotionResponse>>
{
        public bool OnlyActive { get; set; }
}

public class PromotionResponse
{
    public Guid Id { get; set; }
    
        public long MinAmountVnd { get; set; }
    
        public long BonusDiamond { get; set; }
    
    public bool IsActive { get; set; }
    
        public DateTime CreatedAt { get; set; }
}
