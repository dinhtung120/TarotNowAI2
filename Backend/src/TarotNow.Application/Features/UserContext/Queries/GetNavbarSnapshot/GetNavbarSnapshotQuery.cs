using System;
using MediatR;

namespace TarotNow.Application.Features.UserContext.Queries.GetNavbarSnapshot;

public record GetNavbarSnapshotQuery(Guid UserId) : IRequest<NavbarSnapshotDto>;
