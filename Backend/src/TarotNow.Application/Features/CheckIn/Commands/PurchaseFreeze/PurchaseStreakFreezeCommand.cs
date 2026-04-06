using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

/// <summary>
/// Móc Thẻ Kim Cương đi Mua Sự tha thứ (Streak Freeze).
/// Yêu cầu gửi tới Cò Ngân Quỹ (Wallet) nên cần chốt IdempotencyKey cho cứng tay.
/// </summary>
public class PurchaseStreakFreezeCommand : IRequest<PurchaseStreakFreezeResult>
{
    public Guid UserId { get; set; }
    
    // Cái chìa khoá chống kẹt nút (Idempotency), vd: từ UI gen 1 cái Guid mới bấm chọc qua API.
    public string IdempotencyKey { get; set; } = string.Empty;
}
