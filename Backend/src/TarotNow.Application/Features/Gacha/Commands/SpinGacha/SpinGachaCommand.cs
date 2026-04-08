using MediatR;
using System;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

// Command thực hiện quay gacha theo banner.
public class SpinGachaCommand : IRequest<SpinGachaResult>
{
    // Định danh user thực hiện quay.
    public Guid UserId { get; set; }

    // Mã banner cần quay.
    public string BannerCode { get; set; } = string.Empty;

    // Khóa idempotency để chống quay trùng khi retry.
    public string IdempotencyKey { get; set; } = string.Empty;

    // Số lượt quay trong một request.
    public int Count { get; set; } = 1;
}
