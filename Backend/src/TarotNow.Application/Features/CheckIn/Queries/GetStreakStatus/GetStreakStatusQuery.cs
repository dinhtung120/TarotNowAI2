using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

// Query lấy trạng thái streak và khả năng mua freeze của user.
public class GetStreakStatusQuery : IRequest<StreakStatusResult>
{
    // Định danh user cần truy vấn trạng thái streak.
    public Guid UserId { get; set; }
}
