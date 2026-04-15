using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaPools;

/// <summary>
/// Query lấy danh sách pool gacha active.
/// </summary>
public sealed class GetGachaPoolsQuery : IRequest<IReadOnlyList<GachaPoolDto>>
{
    /// <summary>
    /// UserId tùy chọn để trả pity hiện tại.
    /// </summary>
    public Guid? UserId { get; init; }
}
