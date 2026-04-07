

using MediatR;
using System;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

public class CreateDepositOrderCommand : IRequest<CreateDepositOrderResponse>
{
    public Guid UserId { get; set; }
    
    public long AmountVnd { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}

public class CreateDepositOrderResponse
{
        public Guid OrderId { get; set; }
    
    public long AmountVnd { get; set; }
    
        public long DiamondAmount { get; set; }
}
