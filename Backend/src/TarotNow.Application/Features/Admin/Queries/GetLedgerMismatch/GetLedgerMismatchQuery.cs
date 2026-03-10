using MediatR;

namespace TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

public class MismatchRecordDto
{
    public Guid UserId { get; set; }
    public long UserGoldBalance { get; set; }
    public long LedgerGoldBalance { get; set; }
    public long UserDiamondBalance { get; set; }
    public long LedgerDiamondBalance { get; set; }
}

public class GetLedgerMismatchQuery : IRequest<List<MismatchRecordDto>>
{
}
