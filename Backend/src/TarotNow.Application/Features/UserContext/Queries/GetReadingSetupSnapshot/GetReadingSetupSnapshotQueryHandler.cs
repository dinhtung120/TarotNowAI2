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
        var walletTask = _mediator.Send(new GetWalletBalanceQuery(request.UserId), cancellationToken);
        var catalogTask = _mediator.Send(new GetCardsCatalogQuery(), cancellationToken);
        var freeDrawTask = _freeDrawCreditRepository.GetSummaryAsync(request.UserId, cancellationToken);
        await Task.WhenAll(walletTask, catalogTask, freeDrawTask);
        var freeDrawSummary = await freeDrawTask;

        return new ReadingSetupSnapshotDto
        {
            Wallet = await walletTask,
            CardsCatalog = await catalogTask,
            FreeDrawQuotas = new ReadingFreeDrawQuotaDto
            {
                Spread3 = freeDrawSummary.Spread3Count,
                Spread5 = freeDrawSummary.Spread5Count,
                Spread10 = freeDrawSummary.Spread10Count,
            },
        };
    }
}
