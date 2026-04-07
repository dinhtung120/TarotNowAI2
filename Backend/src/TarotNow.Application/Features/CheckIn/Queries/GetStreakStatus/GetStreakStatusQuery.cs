using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

public class GetStreakStatusQuery : IRequest<StreakStatusResult>
{
    public Guid UserId { get; set; }
}
