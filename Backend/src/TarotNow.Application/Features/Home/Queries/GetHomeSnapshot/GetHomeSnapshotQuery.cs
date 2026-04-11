using MediatR;

namespace TarotNow.Application.Features.Home.Queries.GetHomeSnapshot;

public record GetHomeSnapshotQuery : IRequest<HomeSnapshotDto>;
