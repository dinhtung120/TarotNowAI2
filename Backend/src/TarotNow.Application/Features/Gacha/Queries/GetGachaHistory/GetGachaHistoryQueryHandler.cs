

using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

// Handler truy vấn lịch sử quay gacha.
public class GetGachaHistoryQueryHandler : IRequestHandler<GetGachaHistoryQuery, List<GachaHistoryItemDto>>
{
    private readonly IGachaLogRepository _gachaLogRepository;

    /// <summary>
    /// Khởi tạo handler get gacha history.
    /// Luồng xử lý: nhận gacha log repository để đọc lịch sử quay theo user.
    /// </summary>
    public GetGachaHistoryQueryHandler(IGachaLogRepository gachaLogRepository)
    {
        _gachaLogRepository = gachaLogRepository;
    }

    /// <summary>
    /// Xử lý query lấy lịch sử quay.
    /// Luồng xử lý: truy vấn log theo UserId và Limit rồi trả danh sách DTO trực tiếp.
    /// </summary>
    public async Task<List<GachaHistoryItemDto>> Handle(GetGachaHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _gachaLogRepository.GetUserLogsAsync(request.UserId, request.Limit, cancellationToken);
    }
}
