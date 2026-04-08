

using MediatR;
using System;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

// Command tạo đơn nạp tiền (deposit order).
public class CreateDepositOrderCommand : IRequest<CreateDepositOrderResponse>
{
    // Định danh user tạo đơn nạp.
    public Guid UserId { get; set; }

    // Số tiền nạp theo VND.
    public long AmountVnd { get; set; }

    // Khóa idempotency phía client để chống tạo đơn trùng.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// DTO phản hồi sau khi tạo đơn nạp.
public class CreateDepositOrderResponse
{
    // Định danh đơn nạp.
    public Guid OrderId { get; set; }

    // Số tiền VND của đơn nạp.
    public long AmountVnd { get; set; }

    // Tổng kim cương user nhận (đã gồm bonus nếu có).
    public long DiamondAmount { get; set; }
}
