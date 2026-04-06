using MediatR;
using System;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

/// <summary>
/// Cú nháy tra khảo tình trạng Đăng nhập điểm danh có vỡ đập ngày nào không.
/// </summary>
public class GetStreakStatusQuery : IRequest<StreakStatusResult>
{
    public Guid UserId { get; set; }
}
