using MediatR;
using System;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public class SpinGachaCommand : IRequest<SpinGachaResult>
{
    public Guid UserId { get; set; }
    public string BannerCode { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public int Count { get; set; } = 1;
}
