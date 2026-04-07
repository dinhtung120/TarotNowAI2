using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

public class PurchaseStreakFreezeCommand : IRequest<PurchaseStreakFreezeResult>
{
    public Guid UserId { get; set; }
    
    
    public string IdempotencyKey { get; set; } = string.Empty;
}
