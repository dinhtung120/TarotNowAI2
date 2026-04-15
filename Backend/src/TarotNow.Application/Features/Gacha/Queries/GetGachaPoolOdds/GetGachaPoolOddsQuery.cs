using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaPoolOdds;

/// <summary>
/// Query lấy odds theo pool code.
/// </summary>
public sealed class GetGachaPoolOddsQuery : IRequest<GachaPoolOddsDto>
{
    /// <summary>
    /// Mã pool.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Khởi tạo query.
    /// </summary>
    public GetGachaPoolOddsQuery(string poolCode)
    {
        PoolCode = poolCode;
    }
}
