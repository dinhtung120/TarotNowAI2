using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Features.Reader.Queries.ListReaders;

namespace TarotNow.Application.Features.Home.Queries.GetHomeSnapshot;

public class GetHomeSnapshotQueryHandler : IRequestHandler<GetHomeSnapshotQuery, HomeSnapshotDto>
{
    private const int FeaturedPageSize = 4;

    private readonly IMediator _mediator;
    private readonly IUserPresenceTracker _presenceTracker;

    public GetHomeSnapshotQueryHandler(IMediator mediator, IUserPresenceTracker presenceTracker)
    {
        _mediator = mediator;
        _presenceTracker = presenceTracker;
    }

    public async Task<HomeSnapshotDto> Handle(GetHomeSnapshotQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ListReadersQuery
            {
                Page = 1,
                PageSize = FeaturedPageSize
            },
            cancellationToken);

        var readers = result.Readers.ToList();
        foreach (var reader in readers)
        {
            ApplyPresence(reader);
        }

        return new HomeSnapshotDto
        {
            FeaturedReaders = readers,
            TotalCount = result.TotalCount
        };
    }

    private void ApplyPresence(ReaderProfileDto profile)
    {
        if (!_presenceTracker.IsOnline(profile.UserId))
        {
            return;
        }

        var status = profile.Status?.Trim() ?? string.Empty;
        if (string.Equals(status, "offline", System.StringComparison.OrdinalIgnoreCase))
        {
            profile.Status = "online";
        }
    }
}
