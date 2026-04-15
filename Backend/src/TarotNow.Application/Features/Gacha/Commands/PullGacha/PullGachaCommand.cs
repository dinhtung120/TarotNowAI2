using MediatR;

namespace TarotNow.Application.Features.Gacha.Commands.PullGacha;

/// <summary>
/// Command thực hiện pull gacha theo pool.
/// </summary>
public sealed class PullGachaCommand : IRequest<PullGachaResult>
{
    /// <summary>
    /// Định danh user pull.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã pool cần pull.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Khóa idempotency của request.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Số lượt pull.
    /// </summary>
    public int Count { get; init; } = 1;
}
