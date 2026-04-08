using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

// Command thực hiện check-in hằng ngày cho user.
public class DailyCheckInCommand : IRequest<DailyCheckInResult>
{
    // Định danh user thực hiện check-in.
    public Guid UserId { get; set; }
}
