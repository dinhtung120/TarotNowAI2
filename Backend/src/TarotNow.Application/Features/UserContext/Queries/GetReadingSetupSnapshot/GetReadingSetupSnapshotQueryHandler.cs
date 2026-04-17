using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Features.Reading.Queries.GetCardsCatalog;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.UserContext.Queries.GetReadingSetupSnapshot;

public class GetReadingSetupSnapshotQueryHandler : IRequestHandler<GetReadingSetupSnapshotQuery, ReadingSetupSnapshotDto>
{
    private readonly IMediator _mediator;
    private readonly IFreeDrawCreditRepository _freeDrawCreditRepository;

    public GetReadingSetupSnapshotQueryHandler(
        IMediator mediator,
        IFreeDrawCreditRepository freeDrawCreditRepository)
    {
        _mediator = mediator;
        _freeDrawCreditRepository = freeDrawCreditRepository;
    }

    public async Task<ReadingSetupSnapshotDto> Handle(GetReadingSetupSnapshotQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _mediator.Send(new GetWalletBalanceQuery(request.UserId), cancellationToken);
        var catalog = await _mediator.Send(new GetCardsCatalogQuery(), cancellationToken);
        var freeDrawSummary = await _freeDrawCreditRepository.GetSummaryAsync(request.UserId, cancellationToken);

        return new ReadingSetupSnapshotDto
        {
            Wallet = wallet,
            CardsCatalog = catalog,
            FreeDrawQuotas = new ReadingFreeDrawQuotaDto
            {
                Spread3 = freeDrawSummary.Spread3Count,
                Spread5 = freeDrawSummary.Spread5Count,
                Spread10 = freeDrawSummary.Spread10Count,
            },
        };
    }
}
