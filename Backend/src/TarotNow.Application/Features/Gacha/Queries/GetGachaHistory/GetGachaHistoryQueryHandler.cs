

using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;




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
