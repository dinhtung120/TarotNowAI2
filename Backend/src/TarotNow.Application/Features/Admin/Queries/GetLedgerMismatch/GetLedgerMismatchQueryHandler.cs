

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

public class GetLedgerMismatchQueryHandler : IRequestHandler<GetLedgerMismatchQuery, List<MismatchRecordDto>>
{
    private readonly IAdminRepository _adminRepository;

    public GetLedgerMismatchQueryHandler(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

        public async Task<List<MismatchRecordDto>> Handle(GetLedgerMismatchQuery request, CancellationToken cancellationToken)
    {
        
        var entities = await _adminRepository.GetLedgerMismatchesAsync(cancellationToken);
        
        
        return entities.Select(e => new MismatchRecordDto
        {
            UserId = e.UserId,
            UserGoldBalance = e.UserGoldBalance,
            LedgerGoldBalance = e.LedgerGoldBalance,
            UserDiamondBalance = e.UserDiamondBalance,
            LedgerDiamondBalance = e.LedgerDiamondBalance
        }).ToList();
    }
}
