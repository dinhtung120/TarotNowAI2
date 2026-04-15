using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

/// <summary>
/// Query lấy lịch sử reward gacha của user.
/// </summary>
public sealed class GetGachaHistoryQuery : IRequest<GachaHistoryPageDto>
{
    /// <summary>
    /// Định danh user.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Trang hiện tại (1-based).
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Kích thước trang.
    /// </summary>
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Khởi tạo query.
    /// </summary>
    public GetGachaHistoryQuery(Guid userId, int page = 1, int pageSize = 20)
    {
        UserId = userId;
        Page = page;
        PageSize = pageSize;
    }
}
