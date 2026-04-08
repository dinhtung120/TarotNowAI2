using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

// Command mua quyền phục hồi streak bị gãy.
public class PurchaseStreakFreezeCommand : IRequest<PurchaseStreakFreezeResult>
{
    // Định danh user thực hiện mua.
    public Guid UserId { get; set; }

    // Khóa idempotency để chống trừ kim cương trùng.
    public string IdempotencyKey { get; set; } = string.Empty;
}
