using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

public class DailyCheckInCommand : IRequest<DailyCheckInResult>
{
    
    public Guid UserId { get; set; }
}
