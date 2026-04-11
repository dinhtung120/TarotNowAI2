using System;
using MediatR;

namespace TarotNow.Application.Features.UserContext.Queries.GetReadingSetupSnapshot;

public record GetReadingSetupSnapshotQuery(Guid UserId) : IRequest<ReadingSetupSnapshotDto>;
