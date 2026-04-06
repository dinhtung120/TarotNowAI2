using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

/// <summary>
/// Lệnh Kéo máy điểm danh dội vào ngực hệ thống lấy tiền Gold.
/// </summary>
public class DailyCheckInCommand : IRequest<DailyCheckInResult>
{
    // Của ai? (Thường lấy từ Claims nhét vô tại Controller)
    public Guid UserId { get; set; }
}
