using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Features.Reading.Queries.GetCardsCatalog;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

namespace TarotNow.Application.Features.UserContext.Queries.GetReadingSetupSnapshot;

public class GetReadingSetupSnapshotQueryHandler : IRequestHandler<GetReadingSetupSnapshotQuery, ReadingSetupSnapshotDto>
{
    private readonly IMediator _mediator;

    public GetReadingSetupSnapshotQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ReadingSetupSnapshotDto> Handle(GetReadingSetupSnapshotQuery request, CancellationToken cancellationToken)
    {
        var walletTask = _mediator.Send(new GetWalletBalanceQuery(request.UserId), cancellationToken);
        var catalogTask = _mediator.Send(new GetCardsCatalogQuery(), cancellationToken);
        await Task.WhenAll(walletTask, catalogTask);

        return new ReadingSetupSnapshotDto
        {
            Wallet = await walletTask,
            CardsCatalog = await catalogTask
        };
    }
}
