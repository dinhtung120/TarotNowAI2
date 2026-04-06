/*
 * ===================================================================
 * FILE: GetGachaHistoryQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Gacha.Queries.GetGachaHistory
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lấy lịch sử quay Gacha của người dùng.
 * ===================================================================
 */

using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

// Trong MongoDB lưu DTO dạng LogDocument, ở đây ta gọi Repository bằng cách tạo interface trả về DTO
// Tuy nhiên lúc nãy khai báo interface IGachaLogRepository chưa có method GetUserLogsAsync trả về DTO
// Vậy mình cần sửa IGachaLogRepository thêm method đó, hoặc dùng DTO cho gọn.
public class GetGachaHistoryQueryHandler : IRequestHandler<GetGachaHistoryQuery, List<GachaHistoryItemDto>>
{
    private readonly IGachaLogRepository _gachaLogRepository;

    public GetGachaHistoryQueryHandler(IGachaLogRepository gachaLogRepository)
    {
        _gachaLogRepository = gachaLogRepository;
    }

    public async Task<List<GachaHistoryItemDto>> Handle(GetGachaHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _gachaLogRepository.GetUserLogsAsync(request.UserId, request.Limit, cancellationToken);
    }
}
